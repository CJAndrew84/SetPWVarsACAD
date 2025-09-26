# SetPWVarsACAD

Status: Work in Progress

SetPWVarsACAD is a lightweight helper intended to set ProjectWise-related environment variables for AutoCAD-based products before AutoCAD starts. It’s conceptually based on (and inspired by) Dave Brumbaugh’s SetPWVars-CE for MicroStation CONNECT Edition, adapted for the AutoCAD ecosystem.

The goal is to make key ProjectWise context (datasource, document, folder, working directory, user, etc.) available to AutoCAD and related automation (LISP/.NET add-ins, scripts, startup routines) via standard environment variables. This enables more reliable reference resolution, scripting, and downstream automation when launching drawings from ProjectWise Explorer or when running AutoCAD against a ProjectWise working directory.

## What it does (intended)

- Captures ProjectWise context from the launch environment (e.g., when opening a DWG from ProjectWise Explorer).
- Exposes that context as process-level environment variables prior to starting AutoCAD.
- Launches AutoCAD so it inherits those variables, allowing LISP/.NET code and scripts to query them via standard environment variable APIs.

Examples of variables this project intends to expose (names subject to change while WIP):
- PW_DATASOURCE
- PW_USER_NAME
- PW_WORKDIR
- PW_DOC_GUID
- PW_DOC_NAME
- PW_DOC_VERSION
- PW_FOLDER_PATH
- PW_ENVIRONMENT
- PW_STATE

These variables can then be accessed from AutoCAD automation. For example, in AutoLISP:
```lisp
(getenv "PW_DOC_GUID")
(getenv "PW_WORKDIR")
```

## Why

ProjectWise integration with AutoCAD handles file access and references, but custom automation often needs richer context (document metadata, working folder paths, etc.). Providing a consistent set of PW_* variables at process start simplifies:
- Startup scripts that set search paths or logging locations.
- Conditional logic based on document state, environment, or datasource.
- Reference/XREF path handling and consistency across machines.
- External tooling that needs to associate outputs with the active ProjectWise document.

## How it works (high-level, planned)

- A small launcher reads the ProjectWise context from the invocation environment (or from a ProjectWise-provided handoff).
- It sets a defined set of PW_* environment variables.
- It then starts AutoCAD (acad.exe or a vertical) so those variables are available to the AutoCAD process and any children.

Implementation details are evolving while this is a WIP.

## Usage (draft)

There are two common integration patterns. Details and exact steps will be documented as the project stabilizes:

1) Register as a ProjectWise “Application”:
- Configure ProjectWise Application Associations so DWG files open using SetPWVarsACAD, which then launches AutoCAD.
- Command line (example):
  - SetPWVarsACAD.exe -- "C:\Program Files\Autodesk\AutoCAD 2025\acad.exe" "%1"

2) Use as a local launcher:
- Run SetPWVarsACAD to set variables, then start AutoCAD with your target DWG or profile.
- Useful for scripted workflows running against a ProjectWise working directory.

Note: Precise command-line switches, logging options, and variable naming will be finalized as the implementation matures.

## Compatibility (planned)

- AutoCAD: 20xx+ (to be validated)
- ProjectWise Explorer: 20xx+ (to be validated)
- Windows only

## Roadmap

- Finalize the list and naming of PW_* variables.
- Map additional ProjectWise attributes (state, environment, version, etc.).
- Add logging and diagnostics.
- Provide example AutoLISP/.NET snippets for consuming the variables.
- Document Application Association setup in ProjectWise.
- Packaging/installer.

## Acknowledgements

- Based on ideas from Dave Brumbaugh’s SetPWVars-CE for MicroStation CONNECT Edition. This project adapts that concept for AutoCAD.

## Contributing

Contributions and feedback are welcome, especially around:
- Which variables are most useful to expose.
- Testing across different AutoCAD versions and ProjectWise environments.
- Documentation and examples.

## License

TBD
