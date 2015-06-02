# SPWebConfigTransforms

This library has been put together to demonstrate an alternative to using
the SPWebConfigModification class in SharePoint development.

It uses the transform functionality that is available when creating web solutions in
Visual Studio.  This transform functionality is available in the Microsoft.Web.XmlTransform
library.  You can add this to any project using nuget:

    ```shell
    install-package microsoft.web.xdt
    ```

## Reyno.SPWebConfigTransforms.Library

This class library contains the timer job that performs the update to the web.configs
for the requested web applications.

## Reyno.SPWebConfigTransforms.Solution

This is a SharePoint farm solution that deploys the library to a farm, making the timer
job available to your server side code.


