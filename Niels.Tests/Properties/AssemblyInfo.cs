﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// アセンブリに関する一般情報は以下の属性セットをとおして制御されます。
// アセンブリに関連付けられている情報を変更するには、
// これらの属性値を変更してください。
[assembly: AssemblyTitle("Niels.Tests")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Microsoft")]
[assembly: AssemblyProduct("Niels.Tests")]
[assembly: AssemblyCopyright("Copyright © Microsoft 2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// ComVisible を false に設定すると、その型はこのアセンブリ内で COM コンポーネントから 
// 参照できなくなります。このアセンブリ内で COM から型にアクセスする必要がある場合は、 
// その型の ComVisible 属性を true に設定してください。
[assembly: ComVisible(false)]

// このプロジェクトが COM に公開される場合、次の GUID がタイプ ライブラリの ID になります。
[assembly: Guid("b82011b3-88ef-489a-8912-0df7bbf7a399")]

// アセンブリのバージョン情報は、以下の 4 つの値で構成されています:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// すべての値を指定するか、以下のように '*' を使用してビルド番号とリビジョン番号を 
// 既定値にすることができます:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

// 公開先のアセンブリを指定する
// [assembly: InternalsVisibleTo("公開先のアセンブリ名")]
[assembly: InternalsVisibleTo("Niels.MagicBitBoardGenerator.Tests")]
[assembly: InternalsVisibleTo("Niels.Learning")]
[assembly: InternalsVisibleTo("Niels.Learning.Tests")]