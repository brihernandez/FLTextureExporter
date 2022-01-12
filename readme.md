# Freelancer Texture Exporter
![](Screenshots/export.png)

This is a simple command line tool using [Librelancer](https://librelancer.net/) to export the textures from every `.txm`, `.mat`, `.3db`, and `.cmp` file in Freelancer. This includes stuff like ships, stations, effects, solars, and more.

### [Download the latest release here](https://github.com/brihernandez/FLTextureExporter/releases/download/v1.0/FLTextureExporter.zip)

None of this would be possible without the monumental efforts of Librelancer. Please check it out!

## Dependencies
Requires the Librelancer SDK, which can be found here:

https://librelancer.net/downloads.html

*As of January 11 2022, this requires a **daily** build, and not the stable build.*

## How to use

![](Screenshots/program.gif)

Run `FLTextureExporter` in the command line with the following arguments:

* Path to the Librelancer SDK
* Path to the Freelancer directory you want to extract
* Directory to export all the textures to

For example:

```
FLTextureExporter.exe "C:\Freelancer Tools\Librelancer SDK" "C:\Program Files (x86)\Microsoft Games\Freelancer" ".\Export"
```

## Changelog

1.1 (January 12 2022)
- Added option to check CMP files for textures. This will cover lots of misc items that weren't before such as starspheres and cityscapes.

1.0 (January 11 2022)
- Initial release
