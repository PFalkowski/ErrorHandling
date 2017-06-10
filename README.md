Exceptions are not important, untill they occur. Then, they are absolutely central. A mundane task of adding the same behaviour of displaying the error message to the screen or logging it to file or event log is wasteful and can lead to errors. The package reduces the amount of bloat to zero using strategy pattern (a.k.a. policy-based design).

Package provides .NET Standard Infrastructure (.NET Core & .NET Classic comaptible) for creating error handling policy classes with couple of basic implementations:

- Log exception policy
- Collect and ignore exception policy
- Ignore exception policy
- Aggregated wrapper for exception policies
- Customizable exception filter

Contributions are welcomed.