# Quality Open Access Market

## Introduction
Quality Open Access Market (QOAM) is primarily for authors who want to publish their article in open access in a high quality journal and for a reasonable price.

## Building
Building QOAM requires the following steps to be followed:

1. Get a local copy of this repository on your machine (e.g. using `git clone`).
2. Add local versions of config files that contain sensitive information. All configuration sections that contain sensitive information are defined in external files, not in the `Web.config` file themselves. For example, if you look at the `connectionStrings` section in the `Web.config` files of the Website project, you'll find the following:  
`<connectionStrings configSource="ConnectionStrings.Debug.config" />`
Here you can see that the connection strings configuration section is defined in an external file: `ConnectionStrings.Debug.config`. This file should be in the same directory as the `Web.config` file. You should thus create a `ConnectionStrings.Debug.config` file on your local machine that contains the connection strings you want to use. To ease this step, the following zip file contains sample files for every one of the external configuration files: https://www.dropbox.com/s/a8y16utfdtuju7y/Sample-config-files.zip Just extract this zip in your solution folder and then replace the contents of the config files with the values you want to use on your system.
3. Enable the application to be run using HTTPS. This is important as authentication requires HTTPS.
4. Run the application!

If everything works out, you should now see the frontpage.

If you want to run the console applications, follow the same steps but in step 3 replace `Web.config` by `App.config`.

## License
[Apache License 2.0](LICENSE.md)