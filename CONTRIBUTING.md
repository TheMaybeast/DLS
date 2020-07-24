# Welcome!

First of all, I'd like to thank you for being interested in contributing to DLS - Dynamic Lighting System. This CONTRIBUTING.md file defines how development around DLS should occur and be setup.

# How can I contribute?

## Reporting bugs

### Before Reporting a Crash/Bug

1. **Make sure the crash/bug is in fact related to DLS and not any other plugins (including plugins that hook into DLS).** Problems might be related to other plugins loaded which may share the same keys or even be directly hooked into DLS, which may lead to instability.
2. **Make sure the crash/bug is not related to broken configuration files by verifying them.** Sometimes problems can be caused by a broken configuration file, for .xml files I suggest firstly checking it with an (XML Validator)[https://codebeautify.org/xmlvalidator].
3. **Make sure the crash/bug was not previously reported in the [Issues Page](https://github.com/TheMaybeast/DLS/issues).** All bugs reported are being added to the Issues Page, if they were reported in Discord or in any other sources they will also be added to the Issues Page.

### How to Report a Crash/Bug

1. **Access the [New Issue Page](https://github.com/TheMaybeast/DLS/issues/new).**
2. **Utilize a clear and consise title.** Titles like "I have a problem help" aren't exactly helpful, while titles like "X key not working on Y situation" are very helpful.
3. **Describe in the description the steps to reproduce the problem.**
4. **Provide related files.** That includes the vehicle configuration files, DLS.log and RagePluginHook.log and any other files you feel are relevant to the bug.
5. **Include videos/pictures.** Sometimes we'll not be able to reproduce the problem, so if we have a video recording/picture of the specific problem it'll make it easier to track and squash the bug.
6. **Include Suggestions (If able).** If you understand C# and are able to figure out the problem and have suggestions on what's wrong and how to implement the fix that'd be greatly appreciated and should make the fixing process really easier.

## Contributing code

### Branches description

1. **Public branch -** this branch refers to the latest public version of DLS.
2. **Preview branch -** this branch refers to the latest preview version of DLS.
3. **Developer branch -** this branch refers to the latest developer version of DLS.
