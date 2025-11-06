## Setup

To deploy the database, first alter the `MdkLegal` server property in the WebApi project's `appsettings.json` file. Then, navigate to the solution folder in the developer powershell and run:

```dotnet ef database update -p .\helpdesk-support-infrastructure-ef\ -s .\helpdesk-support-webapi\ -c Context -- --environment development```

## Testing

There are two XUnit test libraries suffixed with `.Spec`. They can be run simultaneously and self-clean. If you would like to view the tickets/users committed to storage during the tests, you will have to put a break point on/around the assertion(s) of the desired test. You may have to copy the `appsettings.json` file to the appropriate testing directory.

## Running the application

* Start the back-end project. `MdkLegal.HelpDesk.Support.WebApi` should be the target project. For simplicity, run with the `http` setting. The Swagger UI can be found at `http://localhost:5247/api/index.html`. I included a seed users endpoint that generates a couple of users for your convenience.
* The front-end application is an SPA in React and was built using Next.js. My IDE was Visual Studio Code. Start the front-end project from the root directory with `npm run dev`. Navigate to `http://localhost:3000/ticket-management` for the relevant application.

## Architecture

I implemented this kata with [The Onion Architecture](https://jeffreypalermo.com/2008/07/the-onion-architecture-part-1/). Its entire purpose is to control the flow of dependencies. This enables the domain logic of the application to be read, written, and tested independent of concerns such as storage technology or choice of presentation. Some other key features of this implementation:

* Explicit error handling - I greatly prefer handling errors explicitly. The error handling I included is a simplified version of what I've done in the past. The error codes can be sent to the front end (though I have not done that here) making it easy for the back and front end applications to communicate what happened at any given time.
* Railway programming - Explicit error handling also enables functional programming ideas like railway oriented programming. I have `Result` classes that can be checked for errors or chained together with `.Then` function calls.
* TDD - Tests are written first. This keeps me focused on usage and keeps the high-level code clean.
* Domain-Driven Design - There isn't a complex domain in this project, but it is set up for future expansion. One cool idea here is that the domain representation of a `TicketStatus` is an enum - but the storage representation is three bit columns. This is because the proper way of working with a ticket status is different based on where you're working on it.
* CQRS - While not fully leveraging things like separate read/write databases, there are still two pipelines for commands and queries. Queries run through a simpler project using Dapper, while the commands run through the service layer and use the full EF ORM.
* Task-based - The system is task based, with the exception of the requested "update ticket" functionality. That would ordinarily be split into tasks that communicate *intent*, such as "Alter description". The best example of this is the assign user functionality.
* Command/Query handlers - There are no noun-services (e.g. TicketService) in this application. Instead, there are handlers for each verb. This makes it possible to do things in the future like wrap logging and persistance retry decorators around the handlers.
* RESTful-ish - The API is roughly restful - at least it takes the typical structure. I did not have enough time to build the self-navigation functionality. Future improvements here would include sending arrays of possible tasks to the front end so the front end doesn't need to know *any* endpoint url to navigate the functionality of the application.
* SPA - I am not the best front-end developer in the world. In fact, this is the first front-end I have built in React since maybe 2017. I simply do not possess the same level of expertise on the front end, but I think the fact that the structure of the back end made it possible for me to build this front end in a couple of days.

## Summary

Thank you for reading/skimming this, and for your consideration. If you have any questions, concerns, or issues setting up the application please let me know.
