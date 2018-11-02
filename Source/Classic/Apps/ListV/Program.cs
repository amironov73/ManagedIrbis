﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

class Program
{
  static readonly Regex _regex = new Regex(@"[vV][0-9]+");
  static readonly HashSet<string> _found = new HashSet<string>();

  static void ProcessPattern
    (
      string path,
      string pattern
    )
  {
    var files = Directory.GetFiles(path, pattern,
      SearchOption.AllDirectories);
    foreach (var fileName in files)
    {
      var encoding = Encoding.GetEncoding(1251);
      var text = File.ReadAllText(fileName, encoding);
      var matches = _regex.Matches(text);
      foreach (Match match in matches)
      {
        var value = match.Value;
        if (value.Length != 1)
        {
          _found.Add(value.ToLowerInvariant());
        }
      }
    }
  }

  public static void Main(string[] args)
  {
    if (args.Length < 1)
    {
      return;
    }

    var patterns = new[] {"*.pft", "*.fst", "*.gbl"};
    foreach (var pattern in patterns)
    {
      ProcessPattern(args[0], pattern);
    }

    foreach (var value in _found.OrderBy(_ => _))
    {
      Console.WriteLine(value);
    }
  }
}