Exceptions are not important, untill they occur. Than, they are absolutely central. A mundane task of adding the same behaviour of displaying the error message to the screen or logging it to file or event log is wastefoul and can lead to errors. The package reduces the amount of bloat to single line (+ DI) using strategy pattern (a.k.a. policy-based design).

.NET Standard Infrastructure for creating error handling policy classes with couple of basic implementations:

- Log exception policy
- Collect and ignore exception policy
- Ignore exception policy
- Aggregated wrapper for exception policies

and customizable exception filter.

Contributions are welcomed.
