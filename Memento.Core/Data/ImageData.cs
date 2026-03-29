using System;
using System.IO;

namespace Memento.Core.Data;

public sealed record ImageData(Stream? File, Uri? FilePath);
