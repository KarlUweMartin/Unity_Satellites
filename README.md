## Unity - Satellites

3D visualization of all plublicly documented satellites that orbit earth at this moment. Data provided by NORAD.

[Test it on my website](https://www.karluwemartin.de/satellites/)

#### Usage

Select a dataset from the menu to load satellites. Each satellite can be clicked to reveal its orbit and it's name.

#### Technical Details

``` Unity Version: 6000.3.10f1 ```
Optimized for **WebGL** - but should also work on other platforms.

#### Know Issues

- Orbits of satellites with elliptical, irregular or non-circular orbits are not displayed correctly.
- Large datasets may cause performance issues (especially on mobile devices)

#### Sources

Fetching from: https://celestrak.org/NORAD/elements/
TLE calculation: https://github.com/parzivail/SGP.NET