# showcase
Name: RhoMicro.Unions
Purpose: Generate Discriminated Unions
Main Contributor: Paul Brätz
Source: https://github.com/PaulBraetz/Unions/
License: GPLv3

As per the readme:

- generate rich examination and conversion api
- automatic relation type detection (congruency, superset, subset, intersection)
- generate conversion operators
- generate meaningful api names like `myUnion.IsResult` or `MyUnion.CreateFromResult(result)`
- generate the most efficient impementation for your usecase and optimize against boxing or size constraints

# notes
by 333fred:

@SleepWellPupper I don't have time this evening, but you'll want to ask for advice on restructuring your source generator in this channel. Some immediate red flags:
Using reflection to inspect your types is wrong. You should never be using reflection to load things: inspect the actual symbol models instead.
Please switch over to ForAttributeWithMetadataName
It looks interesting overall, but needs some work to avoid doing a lot of extra work