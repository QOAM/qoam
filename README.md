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
4. Open the Microsoft Management Console (mmc.exe).
5. Add the certificates snap-in and add it managing the "Computer account".
6. Import the `qoam.nl.pfx` private certificate in the **Personal/Certificates** section. Note: you need to contact one of the team members to receive this certificate.
7. Open a command prompt (run as an administrator) and go to the **tools** directory in the solution. In that command prompt run:
`FindPrivateKeyPath.cmd`. This will output the path to the private key (.pfx) file.
8. Navigate to the private key directory and open the "Properties" window of the private key file. Then give the account under which the QOAM website will run read access (for IIS this will probably be `IIS_IUSR`, for IIS express this will your own account).
9. Run the application!

If everything works out, you should now see the frontpage.

If you want to run the console applications, follow the same steps but in step 3 replace `Web.config` by `App.config`.

## License
[Apache License 2.0](LICENSE.md)