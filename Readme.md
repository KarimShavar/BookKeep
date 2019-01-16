### Project overview

Idea behind this application is to allow user to store a collection of books they have read.
User can also track goals of books read they set up for themselfs.
Libary will be filled with information taken from goodreads.com/API.


### Design Process

App will be design following key features of Test Driven Development.
Unit test for most functionality will be created and refactored together with methods.
This will create a well testable and maintainable design, also will be challenging as
for another take on similar design.

### Design Pattern

Project will try to closely follow ModelViewViewModel design pattern.
This should work well with TDD and create a clean architecture to code also leaving it flexible to change.
On top of it project will use the "Meta Pattern" of MVVM that includes a service layer connected to MVVM.


### Data Storage

Data will be stored in two ways. Database will be created with help of EntityFramework to hold app data,
also a local embeded storage will be created using JSON serialization. This should help deepen the understanding
of interfaces and techniques to create flexible code.

### Models

Model of a book should initialy contain all information that can be accessed from goodreads,
what gets to user will be decided later. Leaving room for features and extensions.
Model of a goal needs planning as well. Will not be connected in any way to book,
other then logic to count progress based on number of books in library in a period of time.

*Create a Datetime for when book was read?*

### View - ViewModel

Connection between V an VM will be implemented using viewModelLocator class.
Design of GUI will be closely planned to fit the MVVM model to avoid previous issues.
Inspiration for GUI will be taken from similar functional design ( not priority ).
Commands will be passed through RelayCommand.

### Documentation

Code should be self documented but should also provide comments on bigger methods.
This might help isolate if method is not breaking SRP. All classes should have summary on top.

### Tests

All tests should follow a clear naming conventions and should be categorised.
As this desing will create dozens of tests the should also be categorised in structures similar to main app.
Aim is to achieve 70% + test coverage. As a learning project, this can be increased.
MsTest V2 will be used for testing framework due to familiarity, should also accelerate learning.
Moq will be used for mocking.

### File structure

Files will be separated by type -> ViewModels, Views, Models.
With Models beloging in a BookKeep.Data library together with other services.