using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeSheriff.Secrets;

public class RulesEngine
{
    public static List<GitLeaksRule> GetGitLeaksRules() 
    { 
        var rules = new List<GitLeaksRule>();

        //try
        //{
        string gitLeaksRuleRawContent;

        using (var client = new HttpClient())
        {
            gitLeaksRuleRawContent = client.GetAsync(new Uri("https://raw.githubusercontent.com/gitleaks/gitleaks/master/config/gitleaks.toml")).Result.Content.ReadAsStringAsync().Result;
        }

        var lines = gitLeaksRuleRawContent.Split('\n');

        for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
        {
            if (lines[lineNumber] == "[[rules]]")
            {
                var rule = new GitLeaksRule();

                rule.id = lines[lineNumber + 1].Split(" = ")[1].Trim('"');
                rule.description = lines[lineNumber + 2].Split(" = ")[1].Trim('"');
                //rule.regex = lines[lineNumber + 3].Split(" = ")[1].Trim('\'').Replace("\\\\", "||DOUBLE||").Replace("\\_", "_").Replace("\\r", "").Replace("\\n", "").Replace("||DOUBLE||", "\\\\");
                rule.regex = ConvertGoRegexToCSharpRegex(lines[lineNumber + 3].Split(" = ")[1].Trim('\''));

                rules.Add(rule);
            }
        }
        //}
        //catch
        //{ 
            
        //}

        return rules;
    }

    static string ConvertGoRegexToCSharpRegex(string goRegex)
    {
        goRegex = goRegex.Replace("[0-9a-zA-Z\\-\\_]", "[0-9a-zA-Z-_]");

        return goRegex;
    }
}
