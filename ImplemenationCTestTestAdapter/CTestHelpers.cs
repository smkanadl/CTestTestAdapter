using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestWindow.Data;
using Microsoft.VisualStudio.TestWindow.Extensibility;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImplemenationCTestTestAdapter
{

  
  public static class CTestHelpers
  {
    /// <summary>
    /// returns a enumerable of CTestCases which are queried by ctest -N
    /// </summary>
    /// <param name="solution_dir"></param>
    /// <returns></returns>
    public static IEnumerable<CTestCase> GetTestCases(string solution_dir)
    {
      var process = new Process();
      process.StartInfo = new ProcessStartInfo()
      {
        FileName = "ctest",
        Arguments = "-N",
        CreateNoWindow = true,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        WorkingDirectory = solution_dir,
        WindowStyle = ProcessWindowStyle.Hidden,
        UseShellExecute = false,
  
      };
  
      process.Start();
      
  
      var output = process.StandardOutput.ReadToEnd();
  
      var matches = Regex.Matches(output, @".*#(?<number>[1-9][0-9]*): *(?<testname>.*)");
      foreach (var match in matches)
      {
        var m = match as Match;
        var name = m.Groups["testname"].Value;
        var number = m.Groups["number"].Value;
        yield return new CTestCase() { CMakeBinaryDir = Path.GetFullPath(solution_dir + "\\\\"), Name = name, Number = number };
      }
    }
  }
}
