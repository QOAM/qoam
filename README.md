# Quality Open Access Market

## Introduction
Quality Open Access Market (QOAM) is primarily for authors who want to publish their article in open access in a high quality journal and for a reasonable price.

## Building
Building QOAM requires the following steps to be followed:

1. Get a local copy of this repository on your machine.
2. Get a copy of the database at: https://www.dropbox.com/s/ryw8kt4koe7lawz/OAMarket.mdf It is important to use this preconfigured database as contains the correct triggers, keys and indexes that won't automatically be created by Entity Framework. The database also contains sample journals to jump-start development.
3. In the `Web.config` file in the Website project, replace all "**TODO**" strings with suitable values (e.g. you should set the connection string and the SurfConext credentials.
4. Run the application!

If you want to run the console applications, follow the same steps but in step 3 you replace the "**TODO**" strings in the `App.config` file instead of the `Web.config` file.

## License
[Apache License 2.0](LICENSE.md)