# Teal-Importer
Teal (statically typed lua) importer for unity.

# Requirements
You need to have teal installed and set up in your environment variables.
The download instructions are on the github https://github.com/teal-language/tl

# Limitations
This currently only supports windows, but you could easily be extended to work on other operating systems.

# Usage
When you import/create a new `tl` file, it will automatically be converted to a `TealScriptAsset`, this contains the teal code along with the transpiled code and the errors in the script.

# How It Works
It opens up the command prompt and then tells the `tl` compiler to type check first, it then transpiles to lua, if any issues are found an error will be thrown in the console and the script will be tagged with the error.

# Declaration files
Declaration files '.d.tl' are recognised by the `tl` compiler, they have to be explicitly defined inside `tlconfig.json`.
