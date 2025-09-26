# SetPWVarsACAD

Status: Work in Progress

SetPWVarsACAD is an AutoCAD .NET add-in that reads ProjectWise context for the active drawing and adds those values as Autodesk AEC Property Set properties. It is intended to run automatically when a DWG is opened and also be invokable via an AutoCAD command for on-demand refresh.

This project is based on (and inspired by) Dave Brumbaugh’s SetPWVars-CE for MicroStation, adapted for AutoCAD. It leverages the ProjectWise API (via wrappers) to resolve the active PW document and surfaces selected attributes inside AutoCAD as a Property Set Definition and properties.

## What it does

- On load, ensures a Property Set Definition named “ProjectWise Properties” exists.
- On drawing open (from a ProjectWise-integrated session), resolves the ProjectWise document behind the DWG and prepares properties that mirror PW environment attributes.
- Provides a command (planned) to manually refresh/populate the properties for the current drawing.
- Stores values in AEC Property Data so they can be seen in Properties, tagged, scheduled, or consumed by automation.

Current implementation details (WIP):
- Creates the “ProjectWise Properties” Property Set Definition.
- Applies to AcDb3dSolid objects initially (will broaden in future).
- Uses PWWrapper to select the active document and read metadata.

## Example properties (subject to change while WIP)

Examples of fields this add-in targets (names may evolve):
- PWVAR_VAULTID
- PWVAR_DOCID
- PWVAR_ORIGINALNO
- PWVAR_DOCNAME
- PWVAR_FILENAME
- PWVAR_DOCDESC
- PWVAR_DOCVERSION
- PWVAR_DOCCREATETIME
- PWVAR_DOCUPDATETIME
- PWVAR_DOCFILEUPDATETIME

Note: Some of these are currently implemented as definitions; population/attachment across object types is in progress.

## Why

ProjectWise integration handles file access, but downstream CAD standards, schedules, tags, and automation often need PW context in-drawing. By storing ProjectWise attributes as Property Set data:
- Schedules and tags can reference authoritative PW values.
- Scripts and add-ins can read consistent properties without custom file parsing.
- Auditing, title blocks, and exports can remain aligned with PW metadata.

## Usage

- Automatic on open
  - When the DWG is opened under a ProjectWise-integrated session, the add-in initializes and (as the project matures) will populate the “ProjectWise Properties” Property Set with PW attributes.

- Manual refresh (planned)
  - A custom AutoCAD command will trigger re-evaluation and re-population of the properties on demand.

- Viewing and using the data
  - Use the Properties palette, Style Manager, schedules, or tags to access the “ProjectWise Properties” set and its fields.

## Requirements

- Windows
- AutoCAD-based product compatible with the referenced AutoCAD .NET assemblies (R24.1 family; e.g., AutoCAD 2021) 
- Autodesk AEC Property Data components
  - AutoCAD Architecture/MEP or the appropriate Object Enablers installed
- Bentley ProjectWise Explorer with integration enabled
- ProjectWise API wrappers/libraries (based on Dave Brumbaugh’s wrappers), available at build/runtime

## Build and load

- Clone and restore packages (AutoCAD .NET references are NuGet-based in the csproj).
- Ensure references to PWWrapper/ProjectWise SDK (and any helper libraries) are resolved.
- Build x64 for .NET Framework 4.8.
- Load the built DLL in AutoCAD (NETLOAD) or configure for autoload (Startup Suite or bundle).

On first load, the add-in will create the “ProjectWise Properties” Property Set Definition if it does not exist.

## Roadmap

- Populate properties automatically on DWG open from an integrated PW session.
- Add a user command to re-apply and refresh properties on-demand.
- Expand “applies to” beyond AcDb3dSolid to common drawing object types.
- Finalize the property list and naming.
- Add logging/diagnostics and configuration.
- Provide examples for schedules, tags, and automation consumption.

## Acknowledgements

- Based on ideas from Dave Brumbaugh’s SetPWVars-CE for MicroStation; adapted to AutoCAD.
- Uses ProjectWise API wrappers inspired by “MostOfDavesClasses” and related work.

## License

TBD