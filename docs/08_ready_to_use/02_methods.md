## Methods
Helper utilities methods:

##### Source: Mvc567.Common.Utilities.CryptoFunctions

| Method | Parameters | Return | Description |
| --- | --- | --- | --- |
| MD5Hash | string input | string | Make MD5 hash from string. |
---
##### Source: Mvc567.Common.Utilities.DirectoryFunctions

| Method | Parameters | Return | Description |
| --- | --- | --- | --- |
| AssemblyDirectory (getter) | | string | Get current assembly directory. |
| GenerateRandomName | | string | Generate random name. |
| CreateDirectory | string directory | void | Create directory if doesn't exist |
| CreateFile | string filePath, string fileContent | void | Create file if doesn't exist. |
---
##### Source: Mvc567.Common.Utilities.EnumFunctions

| Method | Parameters | Return | Description |
| --- | --- | --- | --- |
| GetEnumList | Type enumType | Dictionary\<int, string> | Generate dictionary from enum type.  |
---
##### Source: Mvc567.Common.Utilities.FilesFunctions

| Method | Parameters | Return | Description |
| --- | --- | --- | --- |
| SizeSuffix | Int64 value, int decimalPlaces = 1 | string | Get file size suffix based on file size. |
| GetUniqueFileName | | string | Generate unique file name. |
| GetFileExtension | string extension | FileExtensions | Parse file extension from string. |
| GetFileType | FileExtensions fileExtension | FileTypes | Parse file type from string. |
---
##### Source: Mvc567.Common.Utilities.HtmlFunctions

| Method | Parameters | Return | Description |
| --- | --- | --- | --- |
| GetStringFromHtmlContent | IHtmlContent content | string | Get HTML string from HTML content. |
---
##### Source: Mvc567.Common.Utilities.JsonFunctions

| Method | Parameters | Return | Description |
| --- | --- | --- | --- |
| GetJsonObjectByPath | string jsonPath | JObject | Get json object by file path. |
---
##### Source: Mvc567.Common.Utilities.StringFunctions

| Method | Parameters | Return | Description |
| --- | --- | --- | --- |
| SplitWordsByCapitalLetters | string words | string | Split capitalized string to separate words. |