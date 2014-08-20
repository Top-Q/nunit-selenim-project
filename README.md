nunit-selenim-project
=====================

An example for Selenium project written in C# and based on the NUnit framework. The project also includes the Difido reports

Features:
* NUnit framework
* Ability to run from command line
* Simple WebDriver wrapper
* Shiny HTML reports 
* A simple configuration file (SUT)
* Simple infrastructure for web services from type REST

How to use:
* Clone the project from GitHub
* Open with Visual Studio
* Make sure that Visual Studio is configured for downloading NUget packages
* Open the *mysite-tests* project and run the *TestGoogleSearchPage* test located in the *GoogleTests* class
* Right click and execute it (Can be also executed via the TestExplorer
* After successful execution, browse to *Automation-Project\TestResults\Report\current* and click on the *index.html* file to see the results.
