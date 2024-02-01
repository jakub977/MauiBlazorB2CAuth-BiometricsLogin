; ModuleID = 'marshal_methods.armeabi-v7a.ll'
source_filename = "marshal_methods.armeabi-v7a.ll"
target datalayout = "e-m:e-p:32:32-Fi8-i64:64-v128:64:128-a:0:32-n32-S64"
target triple = "armv7-unknown-linux-android21"

%struct.MarshalMethodName = type {
	i64, ; uint64_t id
	ptr ; char* name
}

%struct.MarshalMethodsManagedClass = type {
	i32, ; uint32_t token
	ptr ; MonoClass klass
}

@assembly_image_cache = dso_local local_unnamed_addr global [337 x ptr] zeroinitializer, align 4

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [668 x i32] [
	i32 2616222, ; 0: System.Net.NetworkInformation.dll => 0x27eb9e => 68
	i32 10166715, ; 1: System.Net.NameResolution.dll => 0x9b21bb => 67
	i32 15721112, ; 2: System.Runtime.Intrinsics.dll => 0xefe298 => 108
	i32 32687329, ; 3: Xamarin.AndroidX.Lifecycle.Runtime => 0x1f2c4e1 => 257
	i32 34715100, ; 4: Xamarin.Google.Guava.ListenableFuture.dll => 0x211b5dc => 291
	i32 34839235, ; 5: System.IO.FileSystem.DriveInfo => 0x2139ac3 => 48
	i32 38948123, ; 6: ar\Microsoft.Maui.Controls.resources.dll => 0x2524d1b => 299
	i32 39109920, ; 7: Newtonsoft.Json.dll => 0x254c520 => 213
	i32 39485524, ; 8: System.Net.WebSockets.dll => 0x25a8054 => 80
	i32 42244203, ; 9: he\Microsoft.Maui.Controls.resources.dll => 0x284986b => 308
	i32 42639949, ; 10: System.Threading.Thread => 0x28aa24d => 145
	i32 66541672, ; 11: System.Diagnostics.StackTrace => 0x3f75868 => 30
	i32 67008169, ; 12: zh-Hant\Microsoft.Maui.Controls.resources => 0x3fe76a9 => 332
	i32 68219467, ; 13: System.Security.Cryptography.Primitives => 0x410f24b => 124
	i32 72070932, ; 14: Microsoft.Maui.Graphics.dll => 0x44bb714 => 212
	i32 82292897, ; 15: System.Runtime.CompilerServices.VisualC.dll => 0x4e7b0a1 => 102
	i32 83839681, ; 16: ms\Microsoft.Maui.Controls.resources.dll => 0x4ff4ac1 => 316
	i32 101534019, ; 17: Xamarin.AndroidX.SlidingPaneLayout => 0x60d4943 => 275
	i32 117431740, ; 18: System.Runtime.InteropServices => 0x6ffddbc => 107
	i32 120558881, ; 19: Xamarin.AndroidX.SlidingPaneLayout.dll => 0x72f9521 => 275
	i32 122350210, ; 20: System.Threading.Channels.dll => 0x74aea82 => 139
	i32 134690465, ; 21: Xamarin.Kotlin.StdLib.Jdk7.dll => 0x80736a1 => 295
	i32 136584136, ; 22: zh-Hans\Microsoft.Maui.Controls.resources.dll => 0x8241bc8 => 331
	i32 140062828, ; 23: sk\Microsoft.Maui.Controls.resources.dll => 0x859306c => 324
	i32 142721839, ; 24: System.Net.WebHeaderCollection => 0x881c32f => 77
	i32 149972175, ; 25: System.Security.Cryptography.Primitives.dll => 0x8f064cf => 124
	i32 159306688, ; 26: System.ComponentModel.Annotations => 0x97ed3c0 => 13
	i32 165246403, ; 27: Xamarin.AndroidX.Collection.dll => 0x9d975c3 => 231
	i32 176265551, ; 28: System.ServiceProcess => 0xa81994f => 132
	i32 182336117, ; 29: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0xade3a75 => 277
	i32 184328833, ; 30: System.ValueTuple.dll => 0xafca281 => 151
	i32 205061960, ; 31: System.ComponentModel => 0xc38ff48 => 18
	i32 209399409, ; 32: Xamarin.AndroidX.Browser.dll => 0xc7b2e71 => 229
	i32 220171995, ; 33: System.Diagnostics.Debug => 0xd1f8edb => 26
	i32 230216969, ; 34: Xamarin.AndroidX.Legacy.Support.Core.Utils.dll => 0xdb8d509 => 251
	i32 230752869, ; 35: Microsoft.CSharp.dll => 0xdc10265 => 1
	i32 231409092, ; 36: System.Linq.Parallel => 0xdcb05c4 => 59
	i32 231814094, ; 37: System.Globalization => 0xdd133ce => 42
	i32 244348769, ; 38: Microsoft.AspNetCore.Components.Authorization => 0xe907761 => 177
	i32 246610117, ; 39: System.Reflection.Emit.Lightweight => 0xeb2f8c5 => 91
	i32 254259026, ; 40: Microsoft.AspNetCore.Components.dll => 0xf27af52 => 176
	i32 261689757, ; 41: Xamarin.AndroidX.ConstraintLayout.dll => 0xf99119d => 234
	i32 276479776, ; 42: System.Threading.Timer.dll => 0x107abf20 => 147
	i32 278686392, ; 43: Xamarin.AndroidX.Lifecycle.LiveData.dll => 0x109c6ab8 => 253
	i32 280482487, ; 44: Xamarin.AndroidX.Interpolator => 0x10b7d2b7 => 250
	i32 291076382, ; 45: System.IO.Pipes.AccessControl.dll => 0x1159791e => 54
	i32 298918909, ; 46: System.Net.Ping.dll => 0x11d123fd => 69
	i32 317674968, ; 47: vi\Microsoft.Maui.Controls.resources => 0x12ef55d8 => 329
	i32 318968648, ; 48: Xamarin.AndroidX.Activity.dll => 0x13031348 => 220
	i32 321597661, ; 49: System.Numerics => 0x132b30dd => 83
	i32 321963286, ; 50: fr\Microsoft.Maui.Controls.resources.dll => 0x1330c516 => 307
	i32 342366114, ; 51: Xamarin.AndroidX.Lifecycle.Common => 0x146817a2 => 252
	i32 360082299, ; 52: System.ServiceModel.Web => 0x15766b7b => 131
	i32 367780167, ; 53: System.IO.Pipes => 0x15ebe147 => 55
	i32 374914964, ; 54: System.Transactions.Local => 0x1658bf94 => 149
	i32 375677976, ; 55: System.Net.ServicePoint.dll => 0x16646418 => 74
	i32 379916513, ; 56: System.Threading.Thread.dll => 0x16a510e1 => 145
	i32 385762202, ; 57: System.Memory.dll => 0x16fe439a => 62
	i32 392610295, ; 58: System.Threading.ThreadPool.dll => 0x1766c1f7 => 146
	i32 395744057, ; 59: _Microsoft.Android.Resource.Designer => 0x17969339 => 333
	i32 403441872, ; 60: WindowsBase => 0x180c08d0 => 165
	i32 409257351, ; 61: tr\Microsoft.Maui.Controls.resources.dll => 0x1864c587 => 327
	i32 441335492, ; 62: Xamarin.AndroidX.ConstraintLayout.Core => 0x1a4e3ec4 => 235
	i32 442565967, ; 63: System.Collections => 0x1a61054f => 12
	i32 450948140, ; 64: Xamarin.AndroidX.Fragment.dll => 0x1ae0ec2c => 248
	i32 451504562, ; 65: System.Security.Cryptography.X509Certificates => 0x1ae969b2 => 125
	i32 456227837, ; 66: System.Web.HttpUtility.dll => 0x1b317bfd => 152
	i32 459347974, ; 67: System.Runtime.Serialization.Primitives.dll => 0x1b611806 => 113
	i32 465846621, ; 68: mscorlib => 0x1bc4415d => 166
	i32 469710990, ; 69: System.dll => 0x1bff388e => 164
	i32 476646585, ; 70: Xamarin.AndroidX.Interpolator.dll => 0x1c690cb9 => 250
	i32 485463106, ; 71: Microsoft.IdentityModel.Abstractions => 0x1cef9442 => 202
	i32 486930444, ; 72: Xamarin.AndroidX.LocalBroadcastManager.dll => 0x1d05f80c => 263
	i32 489220957, ; 73: es\Microsoft.Maui.Controls.resources.dll => 0x1d28eb5d => 305
	i32 498788369, ; 74: System.ObjectModel => 0x1dbae811 => 84
	i32 513247710, ; 75: Microsoft.Extensions.Primitives.dll => 0x1e9789de => 200
	i32 526420162, ; 76: System.Transactions.dll => 0x1f6088c2 => 150
	i32 527452488, ; 77: Xamarin.Kotlin.StdLib.Jdk7 => 0x1f704948 => 295
	i32 530272170, ; 78: System.Linq.Queryable => 0x1f9b4faa => 60
	i32 538707440, ; 79: th\Microsoft.Maui.Controls.resources.dll => 0x201c05f0 => 326
	i32 539058512, ; 80: Microsoft.Extensions.Logging => 0x20216150 => 196
	i32 540030774, ; 81: System.IO.FileSystem.dll => 0x20303736 => 51
	i32 545304856, ; 82: System.Runtime.Extensions => 0x2080b118 => 103
	i32 546455878, ; 83: System.Runtime.Serialization.Xml => 0x20924146 => 114
	i32 549171840, ; 84: System.Globalization.Calendars => 0x20bbb280 => 40
	i32 557405415, ; 85: Jsr305Binding => 0x213954e7 => 288
	i32 569601784, ; 86: Xamarin.AndroidX.Window.Extensions.Core.Core => 0x21f36ef8 => 286
	i32 571435654, ; 87: Microsoft.Extensions.FileProviders.Embedded.dll => 0x220f6a86 => 193
	i32 577335427, ; 88: System.Security.Cryptography.Cng => 0x22697083 => 120
	i32 601371474, ; 89: System.IO.IsolatedStorage.dll => 0x23d83352 => 52
	i32 605376203, ; 90: System.IO.Compression.FileSystem => 0x24154ecb => 44
	i32 613668793, ; 91: System.Security.Cryptography.Algorithms => 0x2493d7b9 => 119
	i32 627609679, ; 92: Xamarin.AndroidX.CustomView => 0x2568904f => 240
	i32 627931235, ; 93: nl\Microsoft.Maui.Controls.resources => 0x256d7863 => 318
	i32 639843206, ; 94: Xamarin.AndroidX.Emoji2.ViewsHelper.dll => 0x26233b86 => 246
	i32 643868501, ; 95: System.Net => 0x2660a755 => 81
	i32 662205335, ; 96: System.Text.Encodings.Web.dll => 0x27787397 => 136
	i32 663517072, ; 97: Xamarin.AndroidX.VersionedParcelable => 0x278c7790 => 282
	i32 666292255, ; 98: Xamarin.AndroidX.Arch.Core.Common.dll => 0x27b6d01f => 227
	i32 672442732, ; 99: System.Collections.Concurrent => 0x2814a96c => 8
	i32 683518922, ; 100: System.Net.Security => 0x28bdabca => 73
	i32 690569205, ; 101: System.Xml.Linq.dll => 0x29293ff5 => 155
	i32 691348768, ; 102: Xamarin.KotlinX.Coroutines.Android.dll => 0x29352520 => 297
	i32 693804605, ; 103: System.Windows => 0x295a9e3d => 154
	i32 699345723, ; 104: System.Reflection.Emit => 0x29af2b3b => 92
	i32 700284507, ; 105: Xamarin.Jetbrains.Annotations => 0x29bd7e5b => 292
	i32 700358131, ; 106: System.IO.Compression.ZipFile => 0x29be9df3 => 45
	i32 720511267, ; 107: Xamarin.Kotlin.StdLib.Jdk8 => 0x2af22123 => 296
	i32 722857257, ; 108: System.Runtime.Loader.dll => 0x2b15ed29 => 109
	i32 735137430, ; 109: System.Security.SecureString.dll => 0x2bd14e96 => 129
	i32 752232764, ; 110: System.Diagnostics.Contracts.dll => 0x2cd6293c => 25
	i32 755313932, ; 111: Xamarin.Android.Glide.Annotations.dll => 0x2d052d0c => 217
	i32 759454413, ; 112: System.Net.Requests => 0x2d445acd => 72
	i32 762598435, ; 113: System.IO.Pipes.dll => 0x2d745423 => 55
	i32 775507847, ; 114: System.IO.Compression => 0x2e394f87 => 46
	i32 777317022, ; 115: sk\Microsoft.Maui.Controls.resources => 0x2e54ea9e => 324
	i32 789151979, ; 116: Microsoft.Extensions.Options => 0x2f0980eb => 199
	i32 790371945, ; 117: Xamarin.AndroidX.CustomView.PoolingContainer.dll => 0x2f1c1e69 => 241
	i32 804008546, ; 118: Microsoft.AspNetCore.Components.WebView.Maui => 0x2fec3262 => 182
	i32 804715423, ; 119: System.Data.Common => 0x2ff6fb9f => 22
	i32 807930345, ; 120: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx.dll => 0x302809e9 => 255
	i32 823281589, ; 121: System.Private.Uri.dll => 0x311247b5 => 86
	i32 830298997, ; 122: System.IO.Compression.Brotli => 0x317d5b75 => 43
	i32 832635846, ; 123: System.Xml.XPath.dll => 0x31a103c6 => 160
	i32 834051424, ; 124: System.Net.Quic => 0x31b69d60 => 71
	i32 843511501, ; 125: Xamarin.AndroidX.Print => 0x3246f6cd => 268
	i32 869139383, ; 126: hi\Microsoft.Maui.Controls.resources.dll => 0x33ce03b7 => 309
	i32 873119928, ; 127: Microsoft.VisualBasic => 0x340ac0b8 => 3
	i32 877678880, ; 128: System.Globalization.dll => 0x34505120 => 42
	i32 878954865, ; 129: System.Net.Http.Json => 0x3463c971 => 63
	i32 880668424, ; 130: ru\Microsoft.Maui.Controls.resources.dll => 0x347def08 => 323
	i32 904024072, ; 131: System.ComponentModel.Primitives.dll => 0x35e25008 => 16
	i32 911108515, ; 132: System.IO.MemoryMappedFiles.dll => 0x364e69a3 => 53
	i32 918734561, ; 133: pt-BR\Microsoft.Maui.Controls.resources.dll => 0x36c2c6e1 => 320
	i32 928116545, ; 134: Xamarin.Google.Guava.ListenableFuture => 0x3751ef41 => 291
	i32 952186615, ; 135: System.Runtime.InteropServices.JavaScript.dll => 0x38c136f7 => 105
	i32 955402788, ; 136: Newtonsoft.Json => 0x38f24a24 => 213
	i32 956575887, ; 137: Xamarin.Kotlin.StdLib.Jdk8.dll => 0x3904308f => 296
	i32 961460050, ; 138: it\Microsoft.Maui.Controls.resources.dll => 0x394eb752 => 313
	i32 966729478, ; 139: Xamarin.Google.Crypto.Tink.Android => 0x399f1f06 => 289
	i32 967690846, ; 140: Xamarin.AndroidX.Lifecycle.Common.dll => 0x39adca5e => 252
	i32 975236339, ; 141: System.Diagnostics.Tracing => 0x3a20ecf3 => 34
	i32 975874589, ; 142: System.Xml.XDocument => 0x3a2aaa1d => 158
	i32 986514023, ; 143: System.Private.DataContractSerialization.dll => 0x3acd0267 => 85
	i32 987214855, ; 144: System.Diagnostics.Tools => 0x3ad7b407 => 32
	i32 990727110, ; 145: ro\Microsoft.Maui.Controls.resources.dll => 0x3b0d4bc6 => 322
	i32 992768348, ; 146: System.Collections.dll => 0x3b2c715c => 12
	i32 994442037, ; 147: System.IO.FileSystem => 0x3b45fb35 => 51
	i32 999186168, ; 148: Microsoft.Extensions.FileSystemGlobbing.dll => 0x3b8e5ef8 => 195
	i32 1001831731, ; 149: System.IO.UnmanagedMemoryStream.dll => 0x3bb6bd33 => 56
	i32 1012816738, ; 150: Xamarin.AndroidX.SavedState.dll => 0x3c5e5b62 => 272
	i32 1019214401, ; 151: System.Drawing => 0x3cbffa41 => 36
	i32 1028951442, ; 152: Microsoft.Extensions.DependencyInjection.Abstractions => 0x3d548d92 => 190
	i32 1031528504, ; 153: Xamarin.Google.ErrorProne.Annotations.dll => 0x3d7be038 => 290
	i32 1035644815, ; 154: Xamarin.AndroidX.AppCompat => 0x3dbaaf8f => 225
	i32 1036536393, ; 155: System.Drawing.Primitives.dll => 0x3dc84a49 => 35
	i32 1043950537, ; 156: de\Microsoft.Maui.Controls.resources.dll => 0x3e396bc9 => 303
	i32 1044663988, ; 157: System.Linq.Expressions.dll => 0x3e444eb4 => 58
	i32 1052210849, ; 158: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x3eb776a1 => 259
	i32 1067306892, ; 159: GoogleGson => 0x3f9dcf8c => 174
	i32 1082857460, ; 160: System.ComponentModel.TypeConverter => 0x408b17f4 => 17
	i32 1084122840, ; 161: Xamarin.Kotlin.StdLib => 0x409e66d8 => 293
	i32 1098259244, ; 162: System => 0x41761b2c => 164
	i32 1106973742, ; 163: Microsoft.Extensions.Configuration.FileExtensions.dll => 0x41fb142e => 187
	i32 1108272742, ; 164: sv\Microsoft.Maui.Controls.resources.dll => 0x420ee666 => 325
	i32 1117529484, ; 165: pl\Microsoft.Maui.Controls.resources.dll => 0x429c258c => 319
	i32 1118262833, ; 166: ko\Microsoft.Maui.Controls.resources => 0x42a75631 => 315
	i32 1121599056, ; 167: Xamarin.AndroidX.Lifecycle.Runtime.Ktx.dll => 0x42da3e50 => 258
	i32 1127624469, ; 168: Microsoft.Extensions.Logging.Debug => 0x43362f15 => 198
	i32 1149092582, ; 169: Xamarin.AndroidX.Window => 0x447dc2e6 => 285
	i32 1168523401, ; 170: pt\Microsoft.Maui.Controls.resources => 0x45a64089 => 321
	i32 1170634674, ; 171: System.Web.dll => 0x45c677b2 => 153
	i32 1173126369, ; 172: Microsoft.Extensions.FileProviders.Abstractions.dll => 0x45ec7ce1 => 191
	i32 1175144683, ; 173: Xamarin.AndroidX.VectorDrawable.Animated => 0x460b48eb => 281
	i32 1178241025, ; 174: Xamarin.AndroidX.Navigation.Runtime.dll => 0x463a8801 => 266
	i32 1204270330, ; 175: Xamarin.AndroidX.Arch.Core.Common => 0x47c7b4fa => 227
	i32 1208641965, ; 176: System.Diagnostics.Process => 0x480a69ad => 29
	i32 1219128291, ; 177: System.IO.IsolatedStorage => 0x48aa6be3 => 52
	i32 1228497194, ; 178: MsalAuthInMauiBlazor.dll => 0x4939612a => 0
	i32 1243150071, ; 179: Xamarin.AndroidX.Window.Extensions.Core.Core.dll => 0x4a18f6f7 => 286
	i32 1253011324, ; 180: Microsoft.Win32.Registry => 0x4aaf6f7c => 5
	i32 1260983243, ; 181: cs\Microsoft.Maui.Controls.resources => 0x4b2913cb => 301
	i32 1264511973, ; 182: Xamarin.AndroidX.Startup.StartupRuntime.dll => 0x4b5eebe5 => 276
	i32 1267360935, ; 183: Xamarin.AndroidX.VectorDrawable => 0x4b8a64a7 => 280
	i32 1273260888, ; 184: Xamarin.AndroidX.Collection.Ktx => 0x4be46b58 => 232
	i32 1275534314, ; 185: Xamarin.KotlinX.Coroutines.Android => 0x4c071bea => 297
	i32 1278448581, ; 186: Xamarin.AndroidX.Annotation.Jvm => 0x4c3393c5 => 224
	i32 1293217323, ; 187: Xamarin.AndroidX.DrawerLayout.dll => 0x4d14ee2b => 243
	i32 1308624726, ; 188: hr\Microsoft.Maui.Controls.resources.dll => 0x4e000756 => 310
	i32 1309188875, ; 189: System.Private.DataContractSerialization => 0x4e08a30b => 85
	i32 1322716291, ; 190: Xamarin.AndroidX.Window.dll => 0x4ed70c83 => 285
	i32 1324164729, ; 191: System.Linq => 0x4eed2679 => 61
	i32 1335329327, ; 192: System.Runtime.Serialization.Json.dll => 0x4f97822f => 112
	i32 1336711579, ; 193: zh-HK\Microsoft.Maui.Controls.resources.dll => 0x4fac999b => 330
	i32 1364015309, ; 194: System.IO => 0x514d38cd => 57
	i32 1373134921, ; 195: zh-Hans\Microsoft.Maui.Controls.resources => 0x51d86049 => 331
	i32 1376866003, ; 196: Xamarin.AndroidX.SavedState => 0x52114ed3 => 272
	i32 1379779777, ; 197: System.Resources.ResourceManager => 0x523dc4c1 => 99
	i32 1402170036, ; 198: System.Configuration.dll => 0x53936ab4 => 19
	i32 1406073936, ; 199: Xamarin.AndroidX.CoordinatorLayout => 0x53cefc50 => 236
	i32 1408764838, ; 200: System.Runtime.Serialization.Formatters.dll => 0x53f80ba6 => 111
	i32 1411638395, ; 201: System.Runtime.CompilerServices.Unsafe => 0x5423e47b => 101
	i32 1422545099, ; 202: System.Runtime.CompilerServices.VisualC => 0x54ca50cb => 102
	i32 1430672901, ; 203: ar\Microsoft.Maui.Controls.resources => 0x55465605 => 299
	i32 1434145427, ; 204: System.Runtime.Handles => 0x557b5293 => 104
	i32 1435222561, ; 205: Xamarin.Google.Crypto.Tink.Android.dll => 0x558bc221 => 289
	i32 1439761251, ; 206: System.Net.Quic.dll => 0x55d10363 => 71
	i32 1452070440, ; 207: System.Formats.Asn1.dll => 0x568cd628 => 38
	i32 1453312822, ; 208: System.Diagnostics.Tools.dll => 0x569fcb36 => 32
	i32 1454105418, ; 209: Microsoft.Extensions.FileProviders.Composite => 0x56abe34a => 192
	i32 1457743152, ; 210: System.Runtime.Extensions.dll => 0x56e36530 => 103
	i32 1458022317, ; 211: System.Net.Security.dll => 0x56e7a7ad => 73
	i32 1460893475, ; 212: System.IdentityModel.Tokens.Jwt => 0x57137723 => 214
	i32 1461004990, ; 213: es\Microsoft.Maui.Controls.resources => 0x57152abe => 305
	i32 1461234159, ; 214: System.Collections.Immutable.dll => 0x5718a9ef => 9
	i32 1461719063, ; 215: System.Security.Cryptography.OpenSsl => 0x57201017 => 123
	i32 1462112819, ; 216: System.IO.Compression.dll => 0x57261233 => 46
	i32 1469204771, ; 217: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x57924923 => 226
	i32 1470490898, ; 218: Microsoft.Extensions.Primitives => 0x57a5e912 => 200
	i32 1479771757, ; 219: System.Collections.Immutable => 0x5833866d => 9
	i32 1480492111, ; 220: System.IO.Compression.Brotli.dll => 0x583e844f => 43
	i32 1487239319, ; 221: Microsoft.Win32.Primitives => 0x58a57897 => 4
	i32 1490025113, ; 222: Xamarin.AndroidX.SavedState.SavedState.Ktx.dll => 0x58cffa99 => 273
	i32 1498168481, ; 223: Microsoft.IdentityModel.JsonWebTokens.dll => 0x594c3ca1 => 203
	i32 1521091094, ; 224: Microsoft.Extensions.FileSystemGlobbing => 0x5aaa0216 => 195
	i32 1526286932, ; 225: vi\Microsoft.Maui.Controls.resources.dll => 0x5af94a54 => 329
	i32 1536373174, ; 226: System.Diagnostics.TextWriterTraceListener => 0x5b9331b6 => 31
	i32 1543031311, ; 227: System.Text.RegularExpressions.dll => 0x5bf8ca0f => 138
	i32 1543355203, ; 228: System.Reflection.Emit.dll => 0x5bfdbb43 => 92
	i32 1546581739, ; 229: Microsoft.AspNetCore.Components.WebView.Maui.dll => 0x5c2ef6eb => 182
	i32 1550322496, ; 230: System.Reflection.Extensions.dll => 0x5c680b40 => 93
	i32 1565862583, ; 231: System.IO.FileSystem.Primitives => 0x5d552ab7 => 49
	i32 1566207040, ; 232: System.Threading.Tasks.Dataflow.dll => 0x5d5a6c40 => 141
	i32 1573704789, ; 233: System.Runtime.Serialization.Json => 0x5dccd455 => 112
	i32 1580037396, ; 234: System.Threading.Overlapped => 0x5e2d7514 => 140
	i32 1582372066, ; 235: Xamarin.AndroidX.DocumentFile.dll => 0x5e5114e2 => 242
	i32 1592978981, ; 236: System.Runtime.Serialization.dll => 0x5ef2ee25 => 115
	i32 1597949149, ; 237: Xamarin.Google.ErrorProne.Annotations => 0x5f3ec4dd => 290
	i32 1601112923, ; 238: System.Xml.Serialization => 0x5f6f0b5b => 157
	i32 1604827217, ; 239: System.Net.WebClient => 0x5fa7b851 => 76
	i32 1618516317, ; 240: System.Net.WebSockets.Client.dll => 0x6078995d => 79
	i32 1622152042, ; 241: Xamarin.AndroidX.Loader.dll => 0x60b0136a => 262
	i32 1622358360, ; 242: System.Dynamic.Runtime => 0x60b33958 => 37
	i32 1624863272, ; 243: Xamarin.AndroidX.ViewPager2 => 0x60d97228 => 284
	i32 1632842087, ; 244: Microsoft.Extensions.Configuration.Json => 0x61533167 => 188
	i32 1635184631, ; 245: Xamarin.AndroidX.Emoji2.ViewsHelper => 0x6176eff7 => 246
	i32 1636350590, ; 246: Xamarin.AndroidX.CursorAdapter => 0x6188ba7e => 239
	i32 1639515021, ; 247: System.Net.Http.dll => 0x61b9038d => 64
	i32 1639986890, ; 248: System.Text.RegularExpressions => 0x61c036ca => 138
	i32 1641389582, ; 249: System.ComponentModel.EventBasedAsync.dll => 0x61d59e0e => 15
	i32 1654881142, ; 250: Microsoft.AspNetCore.Components.WebView => 0x62a37b76 => 181
	i32 1657153582, ; 251: System.Runtime => 0x62c6282e => 116
	i32 1658241508, ; 252: Xamarin.AndroidX.Tracing.Tracing.dll => 0x62d6c1e4 => 278
	i32 1658251792, ; 253: Xamarin.Google.Android.Material.dll => 0x62d6ea10 => 287
	i32 1670060433, ; 254: Xamarin.AndroidX.ConstraintLayout => 0x638b1991 => 234
	i32 1675553242, ; 255: System.IO.FileSystem.DriveInfo.dll => 0x63dee9da => 48
	i32 1677501392, ; 256: System.Net.Primitives.dll => 0x63fca3d0 => 70
	i32 1678508291, ; 257: System.Net.WebSockets => 0x640c0103 => 80
	i32 1679018464, ; 258: Blazored.LocalStorage => 0x6413c9e0 => 173
	i32 1679769178, ; 259: System.Security.Cryptography => 0x641f3e5a => 126
	i32 1691477237, ; 260: System.Reflection.Metadata => 0x64d1e4f5 => 94
	i32 1696967625, ; 261: System.Security.Cryptography.Csp => 0x6525abc9 => 121
	i32 1698840827, ; 262: Xamarin.Kotlin.StdLib.Common => 0x654240fb => 294
	i32 1701541528, ; 263: System.Diagnostics.Debug.dll => 0x656b7698 => 26
	i32 1720223769, ; 264: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx => 0x66888819 => 255
	i32 1726116996, ; 265: System.Reflection.dll => 0x66e27484 => 97
	i32 1728033016, ; 266: System.Diagnostics.FileVersionInfo.dll => 0x66ffb0f8 => 28
	i32 1729485958, ; 267: Xamarin.AndroidX.CardView.dll => 0x6715dc86 => 230
	i32 1743415430, ; 268: ca\Microsoft.Maui.Controls.resources => 0x67ea6886 => 300
	i32 1744735666, ; 269: System.Transactions.Local.dll => 0x67fe8db2 => 149
	i32 1746115085, ; 270: System.IO.Pipelines.dll => 0x68139a0d => 215
	i32 1746316138, ; 271: Mono.Android.Export => 0x6816ab6a => 169
	i32 1750313021, ; 272: Microsoft.Win32.Primitives.dll => 0x6853a83d => 4
	i32 1758240030, ; 273: System.Resources.Reader.dll => 0x68cc9d1e => 98
	i32 1760259689, ; 274: Microsoft.AspNetCore.Components.Web.dll => 0x68eb6e69 => 179
	i32 1763938596, ; 275: System.Diagnostics.TraceSource.dll => 0x69239124 => 33
	i32 1765942094, ; 276: System.Reflection.Extensions => 0x6942234e => 93
	i32 1766324549, ; 277: Xamarin.AndroidX.SwipeRefreshLayout => 0x6947f945 => 277
	i32 1770582343, ; 278: Microsoft.Extensions.Logging.dll => 0x6988f147 => 196
	i32 1776026572, ; 279: System.Core.dll => 0x69dc03cc => 21
	i32 1777075843, ; 280: System.Globalization.Extensions.dll => 0x69ec0683 => 41
	i32 1780572499, ; 281: Mono.Android.Runtime.dll => 0x6a216153 => 170
	i32 1782862114, ; 282: ms\Microsoft.Maui.Controls.resources => 0x6a445122 => 316
	i32 1788241197, ; 283: Xamarin.AndroidX.Fragment => 0x6a96652d => 248
	i32 1793755602, ; 284: he\Microsoft.Maui.Controls.resources => 0x6aea89d2 => 308
	i32 1794500907, ; 285: Microsoft.Identity.Client.dll => 0x6af5e92b => 201
	i32 1808609942, ; 286: Xamarin.AndroidX.Loader => 0x6bcd3296 => 262
	i32 1813058853, ; 287: Xamarin.Kotlin.StdLib.dll => 0x6c111525 => 293
	i32 1813201214, ; 288: Xamarin.Google.Android.Material => 0x6c13413e => 287
	i32 1818569960, ; 289: Xamarin.AndroidX.Navigation.UI.dll => 0x6c652ce8 => 267
	i32 1818787751, ; 290: Microsoft.VisualBasic.Core => 0x6c687fa7 => 2
	i32 1824175904, ; 291: System.Text.Encoding.Extensions => 0x6cbab720 => 134
	i32 1824722060, ; 292: System.Runtime.Serialization.Formatters => 0x6cc30c8c => 111
	i32 1827745125, ; 293: Microsoft.AspNetCore.Components.WebAssembly.Authentication => 0x6cf12d65 => 180
	i32 1828688058, ; 294: Microsoft.Extensions.Logging.Abstractions.dll => 0x6cff90ba => 197
	i32 1847515442, ; 295: Xamarin.Android.Glide.Annotations => 0x6e1ed932 => 217
	i32 1853025655, ; 296: sv\Microsoft.Maui.Controls.resources => 0x6e72ed77 => 325
	i32 1858542181, ; 297: System.Linq.Expressions => 0x6ec71a65 => 58
	i32 1870277092, ; 298: System.Reflection.Primitives => 0x6f7a29e4 => 95
	i32 1875935024, ; 299: fr\Microsoft.Maui.Controls.resources => 0x6fd07f30 => 307
	i32 1879696579, ; 300: System.Formats.Tar.dll => 0x7009e4c3 => 39
	i32 1885316902, ; 301: Xamarin.AndroidX.Arch.Core.Runtime.dll => 0x705fa726 => 228
	i32 1888955245, ; 302: System.Diagnostics.Contracts => 0x70972b6d => 25
	i32 1889954781, ; 303: System.Reflection.Metadata.dll => 0x70a66bdd => 94
	i32 1893218855, ; 304: cs\Microsoft.Maui.Controls.resources.dll => 0x70d83a27 => 301
	i32 1898237753, ; 305: System.Reflection.DispatchProxy => 0x7124cf39 => 89
	i32 1900610850, ; 306: System.Resources.ResourceManager.dll => 0x71490522 => 99
	i32 1910275211, ; 307: System.Collections.NonGeneric.dll => 0x71dc7c8b => 10
	i32 1939592360, ; 308: System.Private.Xml.Linq => 0x739bd4a8 => 87
	i32 1953182387, ; 309: id\Microsoft.Maui.Controls.resources.dll => 0x746b32b3 => 312
	i32 1956758971, ; 310: System.Resources.Writer => 0x74a1c5bb => 100
	i32 1961813231, ; 311: Xamarin.AndroidX.Security.SecurityCrypto.dll => 0x74eee4ef => 274
	i32 1968388702, ; 312: Microsoft.Extensions.Configuration.dll => 0x75533a5e => 184
	i32 1983156543, ; 313: Xamarin.Kotlin.StdLib.Common.dll => 0x7634913f => 294
	i32 1985761444, ; 314: Xamarin.Android.Glide.GifDecoder => 0x765c50a4 => 219
	i32 1986222447, ; 315: Microsoft.IdentityModel.Tokens.dll => 0x7663596f => 205
	i32 2003115576, ; 316: el\Microsoft.Maui.Controls.resources => 0x77651e38 => 304
	i32 2011961780, ; 317: System.Buffers.dll => 0x77ec19b4 => 7
	i32 2019465201, ; 318: Xamarin.AndroidX.Lifecycle.ViewModel => 0x785e97f1 => 259
	i32 2031763787, ; 319: Xamarin.Android.Glide => 0x791a414b => 216
	i32 2045470958, ; 320: System.Private.Xml => 0x79eb68ee => 88
	i32 2048278909, ; 321: Microsoft.Extensions.Configuration.Binder.dll => 0x7a16417d => 186
	i32 2055257422, ; 322: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x7a80bd4e => 254
	i32 2060060697, ; 323: System.Windows.dll => 0x7aca0819 => 154
	i32 2066184531, ; 324: de\Microsoft.Maui.Controls.resources => 0x7b277953 => 303
	i32 2070888862, ; 325: System.Diagnostics.TraceSource => 0x7b6f419e => 33
	i32 2072397586, ; 326: Microsoft.Extensions.FileProviders.Physical => 0x7b864712 => 194
	i32 2079903147, ; 327: System.Runtime.dll => 0x7bf8cdab => 116
	i32 2090596640, ; 328: System.Numerics.Vectors => 0x7c9bf920 => 82
	i32 2127167465, ; 329: System.Console => 0x7ec9ffe9 => 20
	i32 2142473426, ; 330: System.Collections.Specialized => 0x7fb38cd2 => 11
	i32 2143790110, ; 331: System.Xml.XmlSerializer.dll => 0x7fc7a41e => 162
	i32 2146852085, ; 332: Microsoft.VisualBasic.dll => 0x7ff65cf5 => 3
	i32 2159891885, ; 333: Microsoft.Maui => 0x80bd55ad => 210
	i32 2169148018, ; 334: hu\Microsoft.Maui.Controls.resources => 0x814a9272 => 311
	i32 2181898931, ; 335: Microsoft.Extensions.Options.dll => 0x820d22b3 => 199
	i32 2192057212, ; 336: Microsoft.Extensions.Logging.Abstractions => 0x82a8237c => 197
	i32 2192166651, ; 337: Microsoft.AspNetCore.Components.Authorization.dll => 0x82a9cefb => 177
	i32 2193016926, ; 338: System.ObjectModel.dll => 0x82b6c85e => 84
	i32 2201107256, ; 339: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 0x83323b38 => 298
	i32 2201231467, ; 340: System.Net.Http => 0x8334206b => 64
	i32 2207618523, ; 341: it\Microsoft.Maui.Controls.resources => 0x839595db => 313
	i32 2217644978, ; 342: Xamarin.AndroidX.VectorDrawable.Animated.dll => 0x842e93b2 => 281
	i32 2222056684, ; 343: System.Threading.Tasks.Parallel => 0x8471e4ec => 143
	i32 2244775296, ; 344: Xamarin.AndroidX.LocalBroadcastManager => 0x85cc8d80 => 263
	i32 2252106437, ; 345: System.Xml.Serialization.dll => 0x863c6ac5 => 157
	i32 2256313426, ; 346: System.Globalization.Extensions => 0x867c9c52 => 41
	i32 2265110946, ; 347: System.Security.AccessControl.dll => 0x8702d9a2 => 117
	i32 2266799131, ; 348: Microsoft.Extensions.Configuration.Abstractions => 0x871c9c1b => 185
	i32 2267999099, ; 349: Xamarin.Android.Glide.DiskLruCache.dll => 0x872eeb7b => 218
	i32 2279755925, ; 350: Xamarin.AndroidX.RecyclerView.dll => 0x87e25095 => 270
	i32 2293034957, ; 351: System.ServiceModel.Web.dll => 0x88acefcd => 131
	i32 2295906218, ; 352: System.Net.Sockets => 0x88d8bfaa => 75
	i32 2298471582, ; 353: System.Net.Mail => 0x88ffe49e => 66
	i32 2303942373, ; 354: nb\Microsoft.Maui.Controls.resources => 0x89535ee5 => 317
	i32 2305521784, ; 355: System.Private.CoreLib.dll => 0x896b7878 => 172
	i32 2311968808, ; 356: Blazored.LocalStorage.dll => 0x89cdd828 => 173
	i32 2315684594, ; 357: Xamarin.AndroidX.Annotation.dll => 0x8a068af2 => 222
	i32 2320631194, ; 358: System.Threading.Tasks.Parallel.dll => 0x8a52059a => 143
	i32 2340441535, ; 359: System.Runtime.InteropServices.RuntimeInformation.dll => 0x8b804dbf => 106
	i32 2344264397, ; 360: System.ValueTuple => 0x8bbaa2cd => 151
	i32 2353062107, ; 361: System.Net.Primitives => 0x8c40e0db => 70
	i32 2366048013, ; 362: hu\Microsoft.Maui.Controls.resources.dll => 0x8d07070d => 311
	i32 2368005991, ; 363: System.Xml.ReaderWriter.dll => 0x8d24e767 => 156
	i32 2369706906, ; 364: Microsoft.IdentityModel.Logging => 0x8d3edb9a => 204
	i32 2371007202, ; 365: Microsoft.Extensions.Configuration => 0x8d52b2e2 => 184
	i32 2378619854, ; 366: System.Security.Cryptography.Csp.dll => 0x8dc6dbce => 121
	i32 2383496789, ; 367: System.Security.Principal.Windows.dll => 0x8e114655 => 127
	i32 2395872292, ; 368: id\Microsoft.Maui.Controls.resources => 0x8ece1c24 => 312
	i32 2401565422, ; 369: System.Web.HttpUtility => 0x8f24faee => 152
	i32 2403452196, ; 370: Xamarin.AndroidX.Emoji2.dll => 0x8f41c524 => 245
	i32 2411328690, ; 371: Microsoft.AspNetCore.Components => 0x8fb9f4b2 => 176
	i32 2421380589, ; 372: System.Threading.Tasks.Dataflow => 0x905355ed => 141
	i32 2423080555, ; 373: Xamarin.AndroidX.Collection.Ktx.dll => 0x906d466b => 232
	i32 2427813419, ; 374: hi\Microsoft.Maui.Controls.resources => 0x90b57e2b => 309
	i32 2435356389, ; 375: System.Console.dll => 0x912896e5 => 20
	i32 2435904999, ; 376: System.ComponentModel.DataAnnotations.dll => 0x9130f5e7 => 14
	i32 2442556106, ; 377: Microsoft.JSInterop.dll => 0x919672ca => 206
	i32 2454642406, ; 378: System.Text.Encoding.dll => 0x924edee6 => 135
	i32 2458678730, ; 379: System.Net.Sockets.dll => 0x928c75ca => 75
	i32 2459001652, ; 380: System.Linq.Parallel.dll => 0x92916334 => 59
	i32 2465532216, ; 381: Xamarin.AndroidX.ConstraintLayout.Core.dll => 0x92f50938 => 235
	i32 2471841756, ; 382: netstandard.dll => 0x93554fdc => 167
	i32 2475788418, ; 383: Java.Interop.dll => 0x93918882 => 168
	i32 2480646305, ; 384: Microsoft.Maui.Controls => 0x93dba8a1 => 208
	i32 2483903535, ; 385: System.ComponentModel.EventBasedAsync => 0x940d5c2f => 15
	i32 2484371297, ; 386: System.Net.ServicePoint => 0x94147f61 => 74
	i32 2490993605, ; 387: System.AppContext.dll => 0x94798bc5 => 6
	i32 2501346920, ; 388: System.Data.DataSetExtensions => 0x95178668 => 23
	i32 2503351294, ; 389: ko\Microsoft.Maui.Controls.resources.dll => 0x95361bfe => 315
	i32 2505896520, ; 390: Xamarin.AndroidX.Lifecycle.Runtime.dll => 0x955cf248 => 257
	i32 2522472828, ; 391: Xamarin.Android.Glide.dll => 0x9659e17c => 216
	i32 2537015816, ; 392: Microsoft.AspNetCore.Authorization => 0x9737ca08 => 175
	i32 2538310050, ; 393: System.Reflection.Emit.Lightweight.dll => 0x974b89a2 => 91
	i32 2550873716, ; 394: hr\Microsoft.Maui.Controls.resources => 0x980b3e74 => 310
	i32 2562349572, ; 395: Microsoft.CSharp => 0x98ba5a04 => 1
	i32 2570120770, ; 396: System.Text.Encodings.Web => 0x9930ee42 => 136
	i32 2576534780, ; 397: ja\Microsoft.Maui.Controls.resources.dll => 0x9992ccfc => 314
	i32 2581783588, ; 398: Xamarin.AndroidX.Lifecycle.Runtime.Ktx => 0x99e2e424 => 258
	i32 2581819634, ; 399: Xamarin.AndroidX.VectorDrawable.dll => 0x99e370f2 => 280
	i32 2585220780, ; 400: System.Text.Encoding.Extensions.dll => 0x9a1756ac => 134
	i32 2585805581, ; 401: System.Net.Ping => 0x9a20430d => 69
	i32 2585813321, ; 402: Microsoft.AspNetCore.Components.Forms => 0x9a206149 => 178
	i32 2589602615, ; 403: System.Threading.ThreadPool => 0x9a5a3337 => 146
	i32 2592341985, ; 404: Microsoft.Extensions.FileProviders.Abstractions => 0x9a83ffe1 => 191
	i32 2593496499, ; 405: pl\Microsoft.Maui.Controls.resources => 0x9a959db3 => 319
	i32 2605712449, ; 406: Xamarin.KotlinX.Coroutines.Core.Jvm => 0x9b500441 => 298
	i32 2615233544, ; 407: Xamarin.AndroidX.Fragment.Ktx => 0x9be14c08 => 249
	i32 2616218305, ; 408: Microsoft.Extensions.Logging.Debug.dll => 0x9bf052c1 => 198
	i32 2617129537, ; 409: System.Private.Xml.dll => 0x9bfe3a41 => 88
	i32 2618712057, ; 410: System.Reflection.TypeExtensions.dll => 0x9c165ff9 => 96
	i32 2620871830, ; 411: Xamarin.AndroidX.CursorAdapter.dll => 0x9c375496 => 239
	i32 2624644809, ; 412: Xamarin.AndroidX.DynamicAnimation => 0x9c70e6c9 => 244
	i32 2626831493, ; 413: ja\Microsoft.Maui.Controls.resources => 0x9c924485 => 314
	i32 2627185994, ; 414: System.Diagnostics.TextWriterTraceListener.dll => 0x9c97ad4a => 31
	i32 2629843544, ; 415: System.IO.Compression.ZipFile.dll => 0x9cc03a58 => 45
	i32 2633051222, ; 416: Xamarin.AndroidX.Lifecycle.LiveData => 0x9cf12c56 => 253
	i32 2640290731, ; 417: Microsoft.IdentityModel.Logging.dll => 0x9d5fa3ab => 204
	i32 2657731189, ; 418: Microsoft.AspNetCore.Components.WebAssembly.Authentication.dll => 0x9e69c275 => 180
	i32 2663391936, ; 419: Xamarin.Android.Glide.DiskLruCache => 0x9ec022c0 => 218
	i32 2663698177, ; 420: System.Runtime.Loader => 0x9ec4cf01 => 109
	i32 2664396074, ; 421: System.Xml.XDocument.dll => 0x9ecf752a => 158
	i32 2665622720, ; 422: System.Drawing.Primitives => 0x9ee22cc0 => 35
	i32 2676780864, ; 423: System.Data.Common.dll => 0x9f8c6f40 => 22
	i32 2686887180, ; 424: System.Runtime.Serialization.Xml.dll => 0xa026a50c => 114
	i32 2692077919, ; 425: Microsoft.AspNetCore.Components.WebView.dll => 0xa075d95f => 181
	i32 2693849962, ; 426: System.IO.dll => 0xa090e36a => 57
	i32 2701096212, ; 427: Xamarin.AndroidX.Tracing.Tracing => 0xa0ff7514 => 278
	i32 2715334215, ; 428: System.Threading.Tasks.dll => 0xa1d8b647 => 144
	i32 2717744543, ; 429: System.Security.Claims => 0xa1fd7d9f => 118
	i32 2719963679, ; 430: System.Security.Cryptography.Cng.dll => 0xa21f5a1f => 120
	i32 2724373263, ; 431: System.Runtime.Numerics.dll => 0xa262a30f => 110
	i32 2732626843, ; 432: Xamarin.AndroidX.Activity => 0xa2e0939b => 220
	i32 2735172069, ; 433: System.Threading.Channels => 0xa30769e5 => 139
	i32 2735631878, ; 434: Microsoft.AspNetCore.Authorization.dll => 0xa30e6e06 => 175
	i32 2737747696, ; 435: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0xa32eb6f0 => 226
	i32 2740051746, ; 436: Microsoft.Identity.Client => 0xa351df22 => 201
	i32 2740698338, ; 437: ca\Microsoft.Maui.Controls.resources.dll => 0xa35bbce2 => 300
	i32 2740948882, ; 438: System.IO.Pipes.AccessControl => 0xa35f8f92 => 54
	i32 2748088231, ; 439: System.Runtime.InteropServices.JavaScript => 0xa3cc7fa7 => 105
	i32 2752995522, ; 440: pt-BR\Microsoft.Maui.Controls.resources => 0xa41760c2 => 320
	i32 2758225723, ; 441: Microsoft.Maui.Controls.Xaml => 0xa4672f3b => 209
	i32 2764765095, ; 442: Microsoft.Maui.dll => 0xa4caf7a7 => 210
	i32 2765824710, ; 443: System.Text.Encoding.CodePages.dll => 0xa4db22c6 => 133
	i32 2770495804, ; 444: Xamarin.Jetbrains.Annotations.dll => 0xa522693c => 292
	i32 2778768386, ; 445: Xamarin.AndroidX.ViewPager.dll => 0xa5a0a402 => 283
	i32 2779977773, ; 446: Xamarin.AndroidX.ResourceInspection.Annotation.dll => 0xa5b3182d => 271
	i32 2785988530, ; 447: th\Microsoft.Maui.Controls.resources => 0xa60ecfb2 => 326
	i32 2788224221, ; 448: Xamarin.AndroidX.Fragment.Ktx.dll => 0xa630ecdd => 249
	i32 2801831435, ; 449: Microsoft.Maui.Graphics => 0xa7008e0b => 212
	i32 2803228030, ; 450: System.Xml.XPath.XDocument.dll => 0xa715dd7e => 159
	i32 2810250172, ; 451: Xamarin.AndroidX.CoordinatorLayout.dll => 0xa78103bc => 236
	i32 2819470561, ; 452: System.Xml.dll => 0xa80db4e1 => 163
	i32 2821205001, ; 453: System.ServiceProcess.dll => 0xa8282c09 => 132
	i32 2821294376, ; 454: Xamarin.AndroidX.ResourceInspection.Annotation => 0xa8298928 => 271
	i32 2824502124, ; 455: System.Xml.XmlDocument => 0xa85a7b6c => 161
	i32 2833784645, ; 456: Microsoft.AspNetCore.Metadata => 0xa8e81f45 => 183
	i32 2838993487, ; 457: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx.dll => 0xa9379a4f => 260
	i32 2849599387, ; 458: System.Threading.Overlapped.dll => 0xa9d96f9b => 140
	i32 2853208004, ; 459: Xamarin.AndroidX.ViewPager => 0xaa107fc4 => 283
	i32 2855708567, ; 460: Xamarin.AndroidX.Transition => 0xaa36a797 => 279
	i32 2861098320, ; 461: Mono.Android.Export.dll => 0xaa88e550 => 169
	i32 2861189240, ; 462: Microsoft.Maui.Essentials => 0xaa8a4878 => 211
	i32 2870099610, ; 463: Xamarin.AndroidX.Activity.Ktx.dll => 0xab123e9a => 221
	i32 2875164099, ; 464: Jsr305Binding.dll => 0xab5f85c3 => 288
	i32 2875220617, ; 465: System.Globalization.Calendars.dll => 0xab606289 => 40
	i32 2884993177, ; 466: Xamarin.AndroidX.ExifInterface => 0xabf58099 => 247
	i32 2887636118, ; 467: System.Net.dll => 0xac1dd496 => 81
	i32 2892341533, ; 468: Microsoft.AspNetCore.Components.Web => 0xac65a11d => 179
	i32 2899753641, ; 469: System.IO.UnmanagedMemoryStream => 0xacd6baa9 => 56
	i32 2900621748, ; 470: System.Dynamic.Runtime.dll => 0xace3f9b4 => 37
	i32 2901442782, ; 471: System.Reflection => 0xacf080de => 97
	i32 2905242038, ; 472: mscorlib.dll => 0xad2a79b6 => 166
	i32 2909740682, ; 473: System.Private.CoreLib => 0xad6f1e8a => 172
	i32 2911054922, ; 474: Microsoft.Extensions.FileProviders.Physical.dll => 0xad832c4a => 194
	i32 2916838712, ; 475: Xamarin.AndroidX.ViewPager2.dll => 0xaddb6d38 => 284
	i32 2919462931, ; 476: System.Numerics.Vectors.dll => 0xae037813 => 82
	i32 2921128767, ; 477: Xamarin.AndroidX.Annotation.Experimental.dll => 0xae1ce33f => 223
	i32 2936416060, ; 478: System.Resources.Reader => 0xaf06273c => 98
	i32 2940926066, ; 479: System.Diagnostics.StackTrace.dll => 0xaf4af872 => 30
	i32 2942453041, ; 480: System.Xml.XPath.XDocument => 0xaf624531 => 159
	i32 2959614098, ; 481: System.ComponentModel.dll => 0xb0682092 => 18
	i32 2968338931, ; 482: System.Security.Principal.Windows => 0xb0ed41f3 => 127
	i32 2972252294, ; 483: System.Security.Cryptography.Algorithms.dll => 0xb128f886 => 119
	i32 2978675010, ; 484: Xamarin.AndroidX.DrawerLayout => 0xb18af942 => 243
	i32 2987532451, ; 485: Xamarin.AndroidX.Security.SecurityCrypto => 0xb21220a3 => 274
	i32 2996846495, ; 486: Xamarin.AndroidX.Lifecycle.Process.dll => 0xb2a03f9f => 256
	i32 3016983068, ; 487: Xamarin.AndroidX.Startup.StartupRuntime => 0xb3d3821c => 276
	i32 3023353419, ; 488: WindowsBase.dll => 0xb434b64b => 165
	i32 3024354802, ; 489: Xamarin.AndroidX.Legacy.Support.Core.Utils => 0xb443fdf2 => 251
	i32 3038032645, ; 490: _Microsoft.Android.Resource.Designer.dll => 0xb514b305 => 333
	i32 3053864966, ; 491: fi\Microsoft.Maui.Controls.resources.dll => 0xb6064806 => 306
	i32 3056245963, ; 492: Xamarin.AndroidX.SavedState.SavedState.Ktx => 0xb62a9ccb => 273
	i32 3057625584, ; 493: Xamarin.AndroidX.Navigation.Common => 0xb63fa9f0 => 264
	i32 3059408633, ; 494: Mono.Android.Runtime => 0xb65adef9 => 170
	i32 3059793426, ; 495: System.ComponentModel.Primitives => 0xb660be12 => 16
	i32 3075834255, ; 496: System.Threading.Tasks => 0xb755818f => 144
	i32 3084678329, ; 497: Microsoft.IdentityModel.Tokens => 0xb7dc74b9 => 205
	i32 3090735792, ; 498: System.Security.Cryptography.X509Certificates.dll => 0xb838e2b0 => 125
	i32 3099732863, ; 499: System.Security.Claims.dll => 0xb8c22b7f => 118
	i32 3103242213, ; 500: MsalAuthInMauiBlazor => 0xb8f7b7e5 => 0
	i32 3103600923, ; 501: System.Formats.Asn1 => 0xb8fd311b => 38
	i32 3111772706, ; 502: System.Runtime.Serialization => 0xb979e222 => 115
	i32 3121463068, ; 503: System.IO.FileSystem.AccessControl.dll => 0xba0dbf1c => 47
	i32 3124832203, ; 504: System.Threading.Tasks.Extensions => 0xba4127cb => 142
	i32 3132293585, ; 505: System.Security.AccessControl => 0xbab301d1 => 117
	i32 3147165239, ; 506: System.Diagnostics.Tracing.dll => 0xbb95ee37 => 34
	i32 3148237826, ; 507: GoogleGson.dll => 0xbba64c02 => 174
	i32 3159123045, ; 508: System.Reflection.Primitives.dll => 0xbc4c6465 => 95
	i32 3160747431, ; 509: System.IO.MemoryMappedFiles => 0xbc652da7 => 53
	i32 3178803400, ; 510: Xamarin.AndroidX.Navigation.Fragment.dll => 0xbd78b0c8 => 265
	i32 3192346100, ; 511: System.Security.SecureString => 0xbe4755f4 => 129
	i32 3193515020, ; 512: System.Web => 0xbe592c0c => 153
	i32 3204380047, ; 513: System.Data.dll => 0xbefef58f => 24
	i32 3209718065, ; 514: System.Xml.XmlDocument.dll => 0xbf506931 => 161
	i32 3211777861, ; 515: Xamarin.AndroidX.DocumentFile => 0xbf6fd745 => 242
	i32 3220365878, ; 516: System.Threading => 0xbff2e236 => 148
	i32 3226221578, ; 517: System.Runtime.Handles.dll => 0xc04c3c0a => 104
	i32 3251039220, ; 518: System.Reflection.DispatchProxy.dll => 0xc1c6ebf4 => 89
	i32 3258312781, ; 519: Xamarin.AndroidX.CardView => 0xc235e84d => 230
	i32 3265493905, ; 520: System.Linq.Queryable.dll => 0xc2a37b91 => 60
	i32 3265893370, ; 521: System.Threading.Tasks.Extensions.dll => 0xc2a993fa => 142
	i32 3277815716, ; 522: System.Resources.Writer.dll => 0xc35f7fa4 => 100
	i32 3279906254, ; 523: Microsoft.Win32.Registry.dll => 0xc37f65ce => 5
	i32 3280506390, ; 524: System.ComponentModel.Annotations.dll => 0xc3888e16 => 13
	i32 3290767353, ; 525: System.Security.Cryptography.Encoding => 0xc4251ff9 => 122
	i32 3299363146, ; 526: System.Text.Encoding => 0xc4a8494a => 135
	i32 3303498502, ; 527: System.Diagnostics.FileVersionInfo => 0xc4e76306 => 28
	i32 3305363605, ; 528: fi\Microsoft.Maui.Controls.resources => 0xc503d895 => 306
	i32 3312457198, ; 529: Microsoft.IdentityModel.JsonWebTokens => 0xc57015ee => 203
	i32 3316684772, ; 530: System.Net.Requests.dll => 0xc5b097e4 => 72
	i32 3317135071, ; 531: Xamarin.AndroidX.CustomView.dll => 0xc5b776df => 240
	i32 3317144872, ; 532: System.Data => 0xc5b79d28 => 24
	i32 3340431453, ; 533: Xamarin.AndroidX.Arch.Core.Runtime => 0xc71af05d => 228
	i32 3345895724, ; 534: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller.dll => 0xc76e512c => 269
	i32 3346324047, ; 535: Xamarin.AndroidX.Navigation.Runtime => 0xc774da4f => 266
	i32 3357674450, ; 536: ru\Microsoft.Maui.Controls.resources => 0xc8220bd2 => 323
	i32 3358260929, ; 537: System.Text.Json => 0xc82afec1 => 137
	i32 3362336904, ; 538: Xamarin.AndroidX.Activity.Ktx => 0xc8693088 => 221
	i32 3362522851, ; 539: Xamarin.AndroidX.Core => 0xc86c06e3 => 237
	i32 3366347497, ; 540: Java.Interop => 0xc8a662e9 => 168
	i32 3374999561, ; 541: Xamarin.AndroidX.RecyclerView => 0xc92a6809 => 270
	i32 3381016424, ; 542: da\Microsoft.Maui.Controls.resources => 0xc9863768 => 302
	i32 3395150330, ; 543: System.Runtime.CompilerServices.Unsafe.dll => 0xca5de1fa => 101
	i32 3403906625, ; 544: System.Security.Cryptography.OpenSsl.dll => 0xcae37e41 => 123
	i32 3405233483, ; 545: Xamarin.AndroidX.CustomView.PoolingContainer => 0xcaf7bd4b => 241
	i32 3406629867, ; 546: Microsoft.Extensions.FileProviders.Composite.dll => 0xcb0d0beb => 192
	i32 3421170118, ; 547: Microsoft.Extensions.Configuration.Binder => 0xcbeae9c6 => 186
	i32 3428513518, ; 548: Microsoft.Extensions.DependencyInjection.dll => 0xcc5af6ee => 189
	i32 3429136800, ; 549: System.Xml => 0xcc6479a0 => 163
	i32 3430777524, ; 550: netstandard => 0xcc7d82b4 => 167
	i32 3441283291, ; 551: Xamarin.AndroidX.DynamicAnimation.dll => 0xcd1dd0db => 244
	i32 3445260447, ; 552: System.Formats.Tar => 0xcd5a809f => 39
	i32 3452344032, ; 553: Microsoft.Maui.Controls.Compatibility.dll => 0xcdc696e0 => 207
	i32 3458724246, ; 554: pt\Microsoft.Maui.Controls.resources.dll => 0xce27f196 => 321
	i32 3464190856, ; 555: Microsoft.AspNetCore.Components.Forms.dll => 0xce7b5b88 => 178
	i32 3471940407, ; 556: System.ComponentModel.TypeConverter.dll => 0xcef19b37 => 17
	i32 3476120550, ; 557: Mono.Android => 0xcf3163e6 => 171
	i32 3484440000, ; 558: ro\Microsoft.Maui.Controls.resources => 0xcfb055c0 => 322
	i32 3485117614, ; 559: System.Text.Json.dll => 0xcfbaacae => 137
	i32 3486566296, ; 560: System.Transactions => 0xcfd0c798 => 150
	i32 3493954962, ; 561: Xamarin.AndroidX.Concurrent.Futures.dll => 0xd0418592 => 233
	i32 3500000672, ; 562: Microsoft.JSInterop => 0xd09dc5a0 => 206
	i32 3509114376, ; 563: System.Xml.Linq => 0xd128d608 => 155
	i32 3515174580, ; 564: System.Security.dll => 0xd1854eb4 => 130
	i32 3530912306, ; 565: System.Configuration => 0xd2757232 => 19
	i32 3539954161, ; 566: System.Net.HttpListener => 0xd2ff69f1 => 65
	i32 3560100363, ; 567: System.Threading.Timer => 0xd432d20b => 147
	i32 3570554715, ; 568: System.IO.FileSystem.AccessControl => 0xd4d2575b => 47
	i32 3580758918, ; 569: zh-HK\Microsoft.Maui.Controls.resources => 0xd56e0b86 => 330
	i32 3592228221, ; 570: zh-Hant\Microsoft.Maui.Controls.resources.dll => 0xd61d0d7d => 332
	i32 3597029428, ; 571: Xamarin.Android.Glide.GifDecoder.dll => 0xd6665034 => 219
	i32 3598340787, ; 572: System.Net.WebSockets.Client => 0xd67a52b3 => 79
	i32 3608519521, ; 573: System.Linq.dll => 0xd715a361 => 61
	i32 3624195450, ; 574: System.Runtime.InteropServices.RuntimeInformation => 0xd804d57a => 106
	i32 3627220390, ; 575: Xamarin.AndroidX.Print.dll => 0xd832fda6 => 268
	i32 3633644679, ; 576: Xamarin.AndroidX.Annotation.Experimental => 0xd8950487 => 223
	i32 3638274909, ; 577: System.IO.FileSystem.Primitives.dll => 0xd8dbab5d => 49
	i32 3641597786, ; 578: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0xd90e5f5a => 254
	i32 3643446276, ; 579: tr\Microsoft.Maui.Controls.resources => 0xd92a9404 => 327
	i32 3643854240, ; 580: Xamarin.AndroidX.Navigation.Fragment => 0xd930cda0 => 265
	i32 3645089577, ; 581: System.ComponentModel.DataAnnotations => 0xd943a729 => 14
	i32 3657292374, ; 582: Microsoft.Extensions.Configuration.Abstractions.dll => 0xd9fdda56 => 185
	i32 3660523487, ; 583: System.Net.NetworkInformation => 0xda2f27df => 68
	i32 3672681054, ; 584: Mono.Android.dll => 0xdae8aa5e => 171
	i32 3682565725, ; 585: Xamarin.AndroidX.Browser => 0xdb7f7e5d => 229
	i32 3684561358, ; 586: Xamarin.AndroidX.Concurrent.Futures => 0xdb9df1ce => 233
	i32 3700591436, ; 587: Microsoft.IdentityModel.Abstractions.dll => 0xdc928b4c => 202
	i32 3700866549, ; 588: System.Net.WebProxy.dll => 0xdc96bdf5 => 78
	i32 3706696989, ; 589: Xamarin.AndroidX.Core.Core.Ktx.dll => 0xdcefb51d => 238
	i32 3716563718, ; 590: System.Runtime.Intrinsics => 0xdd864306 => 108
	i32 3718780102, ; 591: Xamarin.AndroidX.Annotation => 0xdda814c6 => 222
	i32 3722202641, ; 592: Microsoft.Extensions.Configuration.Json.dll => 0xdddc4e11 => 188
	i32 3724971120, ; 593: Xamarin.AndroidX.Navigation.Common.dll => 0xde068c70 => 264
	i32 3732100267, ; 594: System.Net.NameResolution => 0xde7354ab => 67
	i32 3732214720, ; 595: Microsoft.AspNetCore.Metadata.dll => 0xde7513c0 => 183
	i32 3737834244, ; 596: System.Net.Http.Json.dll => 0xdecad304 => 63
	i32 3748608112, ; 597: System.Diagnostics.DiagnosticSource => 0xdf6f3870 => 27
	i32 3751444290, ; 598: System.Xml.XPath => 0xdf9a7f42 => 160
	i32 3751619990, ; 599: da\Microsoft.Maui.Controls.resources.dll => 0xdf9d2d96 => 302
	i32 3758424670, ; 600: Microsoft.Extensions.Configuration.FileExtensions => 0xe005025e => 187
	i32 3786282454, ; 601: Xamarin.AndroidX.Collection => 0xe1ae15d6 => 231
	i32 3792276235, ; 602: System.Collections.NonGeneric => 0xe2098b0b => 10
	i32 3800979733, ; 603: Microsoft.Maui.Controls.Compatibility => 0xe28e5915 => 207
	i32 3802395368, ; 604: System.Collections.Specialized.dll => 0xe2a3f2e8 => 11
	i32 3819260425, ; 605: System.Net.WebProxy => 0xe3a54a09 => 78
	i32 3823082795, ; 606: System.Security.Cryptography.dll => 0xe3df9d2b => 126
	i32 3829621856, ; 607: System.Numerics.dll => 0xe4436460 => 83
	i32 3841636137, ; 608: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 0xe4fab729 => 190
	i32 3844307129, ; 609: System.Net.Mail.dll => 0xe52378b9 => 66
	i32 3849253459, ; 610: System.Runtime.InteropServices.dll => 0xe56ef253 => 107
	i32 3870376305, ; 611: System.Net.HttpListener.dll => 0xe6b14171 => 65
	i32 3873536506, ; 612: System.Security.Principal => 0xe6e179fa => 128
	i32 3875112723, ; 613: System.Security.Cryptography.Encoding.dll => 0xe6f98713 => 122
	i32 3885497537, ; 614: System.Net.WebHeaderCollection.dll => 0xe797fcc1 => 77
	i32 3885922214, ; 615: Xamarin.AndroidX.Transition.dll => 0xe79e77a6 => 279
	i32 3888767677, ; 616: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller => 0xe7c9e2bd => 269
	i32 3896106733, ; 617: System.Collections.Concurrent.dll => 0xe839deed => 8
	i32 3896760992, ; 618: Xamarin.AndroidX.Core.dll => 0xe843daa0 => 237
	i32 3901907137, ; 619: Microsoft.VisualBasic.Core.dll => 0xe89260c1 => 2
	i32 3920221145, ; 620: nl\Microsoft.Maui.Controls.resources.dll => 0xe9a9d3d9 => 318
	i32 3920810846, ; 621: System.IO.Compression.FileSystem.dll => 0xe9b2d35e => 44
	i32 3921031405, ; 622: Xamarin.AndroidX.VersionedParcelable.dll => 0xe9b630ed => 282
	i32 3928044579, ; 623: System.Xml.ReaderWriter => 0xea213423 => 156
	i32 3930554604, ; 624: System.Security.Principal.dll => 0xea4780ec => 128
	i32 3931092270, ; 625: Xamarin.AndroidX.Navigation.UI => 0xea4fb52e => 267
	i32 3945713374, ; 626: System.Data.DataSetExtensions.dll => 0xeb2ecede => 23
	i32 3953953790, ; 627: System.Text.Encoding.CodePages => 0xebac8bfe => 133
	i32 3955647286, ; 628: Xamarin.AndroidX.AppCompat.dll => 0xebc66336 => 225
	i32 3959773229, ; 629: Xamarin.AndroidX.Lifecycle.Process => 0xec05582d => 256
	i32 4003436829, ; 630: System.Diagnostics.Process.dll => 0xee9f991d => 29
	i32 4015948917, ; 631: Xamarin.AndroidX.Annotation.Jvm.dll => 0xef5e8475 => 224
	i32 4023392905, ; 632: System.IO.Pipelines => 0xefd01a89 => 215
	i32 4025784931, ; 633: System.Memory => 0xeff49a63 => 62
	i32 4046471985, ; 634: Microsoft.Maui.Controls.Xaml.dll => 0xf1304331 => 209
	i32 4054681211, ; 635: System.Reflection.Emit.ILGeneration => 0xf1ad867b => 90
	i32 4068434129, ; 636: System.Private.Xml.Linq.dll => 0xf27f60d1 => 87
	i32 4073602200, ; 637: System.Threading.dll => 0xf2ce3c98 => 148
	i32 4091086043, ; 638: el\Microsoft.Maui.Controls.resources.dll => 0xf3d904db => 304
	i32 4094352644, ; 639: Microsoft.Maui.Essentials.dll => 0xf40add04 => 211
	i32 4099507663, ; 640: System.Drawing.dll => 0xf45985cf => 36
	i32 4100113165, ; 641: System.Private.Uri => 0xf462c30d => 86
	i32 4101593132, ; 642: Xamarin.AndroidX.Emoji2 => 0xf479582c => 245
	i32 4103439459, ; 643: uk\Microsoft.Maui.Controls.resources.dll => 0xf4958463 => 328
	i32 4126470640, ; 644: Microsoft.Extensions.DependencyInjection => 0xf5f4f1f0 => 189
	i32 4127667938, ; 645: System.IO.FileSystem.Watcher => 0xf60736e2 => 50
	i32 4130442656, ; 646: System.AppContext => 0xf6318da0 => 6
	i32 4147896353, ; 647: System.Reflection.Emit.ILGeneration.dll => 0xf73be021 => 90
	i32 4150914736, ; 648: uk\Microsoft.Maui.Controls.resources => 0xf769eeb0 => 328
	i32 4151237749, ; 649: System.Core => 0xf76edc75 => 21
	i32 4159265925, ; 650: System.Xml.XmlSerializer => 0xf7e95c85 => 162
	i32 4161255271, ; 651: System.Reflection.TypeExtensions => 0xf807b767 => 96
	i32 4164802419, ; 652: System.IO.FileSystem.Watcher.dll => 0xf83dd773 => 50
	i32 4181436372, ; 653: System.Runtime.Serialization.Primitives => 0xf93ba7d4 => 113
	i32 4182413190, ; 654: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0xf94a8f86 => 261
	i32 4185676441, ; 655: System.Security => 0xf97c5a99 => 130
	i32 4196529839, ; 656: System.Net.WebClient.dll => 0xfa21f6af => 76
	i32 4213026141, ; 657: System.Diagnostics.DiagnosticSource.dll => 0xfb1dad5d => 27
	i32 4249188766, ; 658: nb\Microsoft.Maui.Controls.resources.dll => 0xfd45799e => 317
	i32 4256097574, ; 659: Xamarin.AndroidX.Core.Core.Ktx => 0xfdaee526 => 238
	i32 4258378803, ; 660: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx => 0xfdd1b433 => 260
	i32 4260525087, ; 661: System.Buffers => 0xfdf2741f => 7
	i32 4263231520, ; 662: System.IdentityModel.Tokens.Jwt.dll => 0xfe1bc020 => 214
	i32 4271975918, ; 663: Microsoft.Maui.Controls.dll => 0xfea12dee => 208
	i32 4274976490, ; 664: System.Runtime.Numerics => 0xfecef6ea => 110
	i32 4292120959, ; 665: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xffd4917f => 261
	i32 4294648842, ; 666: Microsoft.Extensions.FileProviders.Embedded => 0xfffb240a => 193
	i32 4294763496 ; 667: Xamarin.AndroidX.ExifInterface.dll => 0xfffce3e8 => 247
], align 4

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [668 x i32] [
	i32 68, ; 0
	i32 67, ; 1
	i32 108, ; 2
	i32 257, ; 3
	i32 291, ; 4
	i32 48, ; 5
	i32 299, ; 6
	i32 213, ; 7
	i32 80, ; 8
	i32 308, ; 9
	i32 145, ; 10
	i32 30, ; 11
	i32 332, ; 12
	i32 124, ; 13
	i32 212, ; 14
	i32 102, ; 15
	i32 316, ; 16
	i32 275, ; 17
	i32 107, ; 18
	i32 275, ; 19
	i32 139, ; 20
	i32 295, ; 21
	i32 331, ; 22
	i32 324, ; 23
	i32 77, ; 24
	i32 124, ; 25
	i32 13, ; 26
	i32 231, ; 27
	i32 132, ; 28
	i32 277, ; 29
	i32 151, ; 30
	i32 18, ; 31
	i32 229, ; 32
	i32 26, ; 33
	i32 251, ; 34
	i32 1, ; 35
	i32 59, ; 36
	i32 42, ; 37
	i32 177, ; 38
	i32 91, ; 39
	i32 176, ; 40
	i32 234, ; 41
	i32 147, ; 42
	i32 253, ; 43
	i32 250, ; 44
	i32 54, ; 45
	i32 69, ; 46
	i32 329, ; 47
	i32 220, ; 48
	i32 83, ; 49
	i32 307, ; 50
	i32 252, ; 51
	i32 131, ; 52
	i32 55, ; 53
	i32 149, ; 54
	i32 74, ; 55
	i32 145, ; 56
	i32 62, ; 57
	i32 146, ; 58
	i32 333, ; 59
	i32 165, ; 60
	i32 327, ; 61
	i32 235, ; 62
	i32 12, ; 63
	i32 248, ; 64
	i32 125, ; 65
	i32 152, ; 66
	i32 113, ; 67
	i32 166, ; 68
	i32 164, ; 69
	i32 250, ; 70
	i32 202, ; 71
	i32 263, ; 72
	i32 305, ; 73
	i32 84, ; 74
	i32 200, ; 75
	i32 150, ; 76
	i32 295, ; 77
	i32 60, ; 78
	i32 326, ; 79
	i32 196, ; 80
	i32 51, ; 81
	i32 103, ; 82
	i32 114, ; 83
	i32 40, ; 84
	i32 288, ; 85
	i32 286, ; 86
	i32 193, ; 87
	i32 120, ; 88
	i32 52, ; 89
	i32 44, ; 90
	i32 119, ; 91
	i32 240, ; 92
	i32 318, ; 93
	i32 246, ; 94
	i32 81, ; 95
	i32 136, ; 96
	i32 282, ; 97
	i32 227, ; 98
	i32 8, ; 99
	i32 73, ; 100
	i32 155, ; 101
	i32 297, ; 102
	i32 154, ; 103
	i32 92, ; 104
	i32 292, ; 105
	i32 45, ; 106
	i32 296, ; 107
	i32 109, ; 108
	i32 129, ; 109
	i32 25, ; 110
	i32 217, ; 111
	i32 72, ; 112
	i32 55, ; 113
	i32 46, ; 114
	i32 324, ; 115
	i32 199, ; 116
	i32 241, ; 117
	i32 182, ; 118
	i32 22, ; 119
	i32 255, ; 120
	i32 86, ; 121
	i32 43, ; 122
	i32 160, ; 123
	i32 71, ; 124
	i32 268, ; 125
	i32 309, ; 126
	i32 3, ; 127
	i32 42, ; 128
	i32 63, ; 129
	i32 323, ; 130
	i32 16, ; 131
	i32 53, ; 132
	i32 320, ; 133
	i32 291, ; 134
	i32 105, ; 135
	i32 213, ; 136
	i32 296, ; 137
	i32 313, ; 138
	i32 289, ; 139
	i32 252, ; 140
	i32 34, ; 141
	i32 158, ; 142
	i32 85, ; 143
	i32 32, ; 144
	i32 322, ; 145
	i32 12, ; 146
	i32 51, ; 147
	i32 195, ; 148
	i32 56, ; 149
	i32 272, ; 150
	i32 36, ; 151
	i32 190, ; 152
	i32 290, ; 153
	i32 225, ; 154
	i32 35, ; 155
	i32 303, ; 156
	i32 58, ; 157
	i32 259, ; 158
	i32 174, ; 159
	i32 17, ; 160
	i32 293, ; 161
	i32 164, ; 162
	i32 187, ; 163
	i32 325, ; 164
	i32 319, ; 165
	i32 315, ; 166
	i32 258, ; 167
	i32 198, ; 168
	i32 285, ; 169
	i32 321, ; 170
	i32 153, ; 171
	i32 191, ; 172
	i32 281, ; 173
	i32 266, ; 174
	i32 227, ; 175
	i32 29, ; 176
	i32 52, ; 177
	i32 0, ; 178
	i32 286, ; 179
	i32 5, ; 180
	i32 301, ; 181
	i32 276, ; 182
	i32 280, ; 183
	i32 232, ; 184
	i32 297, ; 185
	i32 224, ; 186
	i32 243, ; 187
	i32 310, ; 188
	i32 85, ; 189
	i32 285, ; 190
	i32 61, ; 191
	i32 112, ; 192
	i32 330, ; 193
	i32 57, ; 194
	i32 331, ; 195
	i32 272, ; 196
	i32 99, ; 197
	i32 19, ; 198
	i32 236, ; 199
	i32 111, ; 200
	i32 101, ; 201
	i32 102, ; 202
	i32 299, ; 203
	i32 104, ; 204
	i32 289, ; 205
	i32 71, ; 206
	i32 38, ; 207
	i32 32, ; 208
	i32 192, ; 209
	i32 103, ; 210
	i32 73, ; 211
	i32 214, ; 212
	i32 305, ; 213
	i32 9, ; 214
	i32 123, ; 215
	i32 46, ; 216
	i32 226, ; 217
	i32 200, ; 218
	i32 9, ; 219
	i32 43, ; 220
	i32 4, ; 221
	i32 273, ; 222
	i32 203, ; 223
	i32 195, ; 224
	i32 329, ; 225
	i32 31, ; 226
	i32 138, ; 227
	i32 92, ; 228
	i32 182, ; 229
	i32 93, ; 230
	i32 49, ; 231
	i32 141, ; 232
	i32 112, ; 233
	i32 140, ; 234
	i32 242, ; 235
	i32 115, ; 236
	i32 290, ; 237
	i32 157, ; 238
	i32 76, ; 239
	i32 79, ; 240
	i32 262, ; 241
	i32 37, ; 242
	i32 284, ; 243
	i32 188, ; 244
	i32 246, ; 245
	i32 239, ; 246
	i32 64, ; 247
	i32 138, ; 248
	i32 15, ; 249
	i32 181, ; 250
	i32 116, ; 251
	i32 278, ; 252
	i32 287, ; 253
	i32 234, ; 254
	i32 48, ; 255
	i32 70, ; 256
	i32 80, ; 257
	i32 173, ; 258
	i32 126, ; 259
	i32 94, ; 260
	i32 121, ; 261
	i32 294, ; 262
	i32 26, ; 263
	i32 255, ; 264
	i32 97, ; 265
	i32 28, ; 266
	i32 230, ; 267
	i32 300, ; 268
	i32 149, ; 269
	i32 215, ; 270
	i32 169, ; 271
	i32 4, ; 272
	i32 98, ; 273
	i32 179, ; 274
	i32 33, ; 275
	i32 93, ; 276
	i32 277, ; 277
	i32 196, ; 278
	i32 21, ; 279
	i32 41, ; 280
	i32 170, ; 281
	i32 316, ; 282
	i32 248, ; 283
	i32 308, ; 284
	i32 201, ; 285
	i32 262, ; 286
	i32 293, ; 287
	i32 287, ; 288
	i32 267, ; 289
	i32 2, ; 290
	i32 134, ; 291
	i32 111, ; 292
	i32 180, ; 293
	i32 197, ; 294
	i32 217, ; 295
	i32 325, ; 296
	i32 58, ; 297
	i32 95, ; 298
	i32 307, ; 299
	i32 39, ; 300
	i32 228, ; 301
	i32 25, ; 302
	i32 94, ; 303
	i32 301, ; 304
	i32 89, ; 305
	i32 99, ; 306
	i32 10, ; 307
	i32 87, ; 308
	i32 312, ; 309
	i32 100, ; 310
	i32 274, ; 311
	i32 184, ; 312
	i32 294, ; 313
	i32 219, ; 314
	i32 205, ; 315
	i32 304, ; 316
	i32 7, ; 317
	i32 259, ; 318
	i32 216, ; 319
	i32 88, ; 320
	i32 186, ; 321
	i32 254, ; 322
	i32 154, ; 323
	i32 303, ; 324
	i32 33, ; 325
	i32 194, ; 326
	i32 116, ; 327
	i32 82, ; 328
	i32 20, ; 329
	i32 11, ; 330
	i32 162, ; 331
	i32 3, ; 332
	i32 210, ; 333
	i32 311, ; 334
	i32 199, ; 335
	i32 197, ; 336
	i32 177, ; 337
	i32 84, ; 338
	i32 298, ; 339
	i32 64, ; 340
	i32 313, ; 341
	i32 281, ; 342
	i32 143, ; 343
	i32 263, ; 344
	i32 157, ; 345
	i32 41, ; 346
	i32 117, ; 347
	i32 185, ; 348
	i32 218, ; 349
	i32 270, ; 350
	i32 131, ; 351
	i32 75, ; 352
	i32 66, ; 353
	i32 317, ; 354
	i32 172, ; 355
	i32 173, ; 356
	i32 222, ; 357
	i32 143, ; 358
	i32 106, ; 359
	i32 151, ; 360
	i32 70, ; 361
	i32 311, ; 362
	i32 156, ; 363
	i32 204, ; 364
	i32 184, ; 365
	i32 121, ; 366
	i32 127, ; 367
	i32 312, ; 368
	i32 152, ; 369
	i32 245, ; 370
	i32 176, ; 371
	i32 141, ; 372
	i32 232, ; 373
	i32 309, ; 374
	i32 20, ; 375
	i32 14, ; 376
	i32 206, ; 377
	i32 135, ; 378
	i32 75, ; 379
	i32 59, ; 380
	i32 235, ; 381
	i32 167, ; 382
	i32 168, ; 383
	i32 208, ; 384
	i32 15, ; 385
	i32 74, ; 386
	i32 6, ; 387
	i32 23, ; 388
	i32 315, ; 389
	i32 257, ; 390
	i32 216, ; 391
	i32 175, ; 392
	i32 91, ; 393
	i32 310, ; 394
	i32 1, ; 395
	i32 136, ; 396
	i32 314, ; 397
	i32 258, ; 398
	i32 280, ; 399
	i32 134, ; 400
	i32 69, ; 401
	i32 178, ; 402
	i32 146, ; 403
	i32 191, ; 404
	i32 319, ; 405
	i32 298, ; 406
	i32 249, ; 407
	i32 198, ; 408
	i32 88, ; 409
	i32 96, ; 410
	i32 239, ; 411
	i32 244, ; 412
	i32 314, ; 413
	i32 31, ; 414
	i32 45, ; 415
	i32 253, ; 416
	i32 204, ; 417
	i32 180, ; 418
	i32 218, ; 419
	i32 109, ; 420
	i32 158, ; 421
	i32 35, ; 422
	i32 22, ; 423
	i32 114, ; 424
	i32 181, ; 425
	i32 57, ; 426
	i32 278, ; 427
	i32 144, ; 428
	i32 118, ; 429
	i32 120, ; 430
	i32 110, ; 431
	i32 220, ; 432
	i32 139, ; 433
	i32 175, ; 434
	i32 226, ; 435
	i32 201, ; 436
	i32 300, ; 437
	i32 54, ; 438
	i32 105, ; 439
	i32 320, ; 440
	i32 209, ; 441
	i32 210, ; 442
	i32 133, ; 443
	i32 292, ; 444
	i32 283, ; 445
	i32 271, ; 446
	i32 326, ; 447
	i32 249, ; 448
	i32 212, ; 449
	i32 159, ; 450
	i32 236, ; 451
	i32 163, ; 452
	i32 132, ; 453
	i32 271, ; 454
	i32 161, ; 455
	i32 183, ; 456
	i32 260, ; 457
	i32 140, ; 458
	i32 283, ; 459
	i32 279, ; 460
	i32 169, ; 461
	i32 211, ; 462
	i32 221, ; 463
	i32 288, ; 464
	i32 40, ; 465
	i32 247, ; 466
	i32 81, ; 467
	i32 179, ; 468
	i32 56, ; 469
	i32 37, ; 470
	i32 97, ; 471
	i32 166, ; 472
	i32 172, ; 473
	i32 194, ; 474
	i32 284, ; 475
	i32 82, ; 476
	i32 223, ; 477
	i32 98, ; 478
	i32 30, ; 479
	i32 159, ; 480
	i32 18, ; 481
	i32 127, ; 482
	i32 119, ; 483
	i32 243, ; 484
	i32 274, ; 485
	i32 256, ; 486
	i32 276, ; 487
	i32 165, ; 488
	i32 251, ; 489
	i32 333, ; 490
	i32 306, ; 491
	i32 273, ; 492
	i32 264, ; 493
	i32 170, ; 494
	i32 16, ; 495
	i32 144, ; 496
	i32 205, ; 497
	i32 125, ; 498
	i32 118, ; 499
	i32 0, ; 500
	i32 38, ; 501
	i32 115, ; 502
	i32 47, ; 503
	i32 142, ; 504
	i32 117, ; 505
	i32 34, ; 506
	i32 174, ; 507
	i32 95, ; 508
	i32 53, ; 509
	i32 265, ; 510
	i32 129, ; 511
	i32 153, ; 512
	i32 24, ; 513
	i32 161, ; 514
	i32 242, ; 515
	i32 148, ; 516
	i32 104, ; 517
	i32 89, ; 518
	i32 230, ; 519
	i32 60, ; 520
	i32 142, ; 521
	i32 100, ; 522
	i32 5, ; 523
	i32 13, ; 524
	i32 122, ; 525
	i32 135, ; 526
	i32 28, ; 527
	i32 306, ; 528
	i32 203, ; 529
	i32 72, ; 530
	i32 240, ; 531
	i32 24, ; 532
	i32 228, ; 533
	i32 269, ; 534
	i32 266, ; 535
	i32 323, ; 536
	i32 137, ; 537
	i32 221, ; 538
	i32 237, ; 539
	i32 168, ; 540
	i32 270, ; 541
	i32 302, ; 542
	i32 101, ; 543
	i32 123, ; 544
	i32 241, ; 545
	i32 192, ; 546
	i32 186, ; 547
	i32 189, ; 548
	i32 163, ; 549
	i32 167, ; 550
	i32 244, ; 551
	i32 39, ; 552
	i32 207, ; 553
	i32 321, ; 554
	i32 178, ; 555
	i32 17, ; 556
	i32 171, ; 557
	i32 322, ; 558
	i32 137, ; 559
	i32 150, ; 560
	i32 233, ; 561
	i32 206, ; 562
	i32 155, ; 563
	i32 130, ; 564
	i32 19, ; 565
	i32 65, ; 566
	i32 147, ; 567
	i32 47, ; 568
	i32 330, ; 569
	i32 332, ; 570
	i32 219, ; 571
	i32 79, ; 572
	i32 61, ; 573
	i32 106, ; 574
	i32 268, ; 575
	i32 223, ; 576
	i32 49, ; 577
	i32 254, ; 578
	i32 327, ; 579
	i32 265, ; 580
	i32 14, ; 581
	i32 185, ; 582
	i32 68, ; 583
	i32 171, ; 584
	i32 229, ; 585
	i32 233, ; 586
	i32 202, ; 587
	i32 78, ; 588
	i32 238, ; 589
	i32 108, ; 590
	i32 222, ; 591
	i32 188, ; 592
	i32 264, ; 593
	i32 67, ; 594
	i32 183, ; 595
	i32 63, ; 596
	i32 27, ; 597
	i32 160, ; 598
	i32 302, ; 599
	i32 187, ; 600
	i32 231, ; 601
	i32 10, ; 602
	i32 207, ; 603
	i32 11, ; 604
	i32 78, ; 605
	i32 126, ; 606
	i32 83, ; 607
	i32 190, ; 608
	i32 66, ; 609
	i32 107, ; 610
	i32 65, ; 611
	i32 128, ; 612
	i32 122, ; 613
	i32 77, ; 614
	i32 279, ; 615
	i32 269, ; 616
	i32 8, ; 617
	i32 237, ; 618
	i32 2, ; 619
	i32 318, ; 620
	i32 44, ; 621
	i32 282, ; 622
	i32 156, ; 623
	i32 128, ; 624
	i32 267, ; 625
	i32 23, ; 626
	i32 133, ; 627
	i32 225, ; 628
	i32 256, ; 629
	i32 29, ; 630
	i32 224, ; 631
	i32 215, ; 632
	i32 62, ; 633
	i32 209, ; 634
	i32 90, ; 635
	i32 87, ; 636
	i32 148, ; 637
	i32 304, ; 638
	i32 211, ; 639
	i32 36, ; 640
	i32 86, ; 641
	i32 245, ; 642
	i32 328, ; 643
	i32 189, ; 644
	i32 50, ; 645
	i32 6, ; 646
	i32 90, ; 647
	i32 328, ; 648
	i32 21, ; 649
	i32 162, ; 650
	i32 96, ; 651
	i32 50, ; 652
	i32 113, ; 653
	i32 261, ; 654
	i32 130, ; 655
	i32 76, ; 656
	i32 27, ; 657
	i32 317, ; 658
	i32 238, ; 659
	i32 260, ; 660
	i32 7, ; 661
	i32 214, ; 662
	i32 208, ; 663
	i32 110, ; 664
	i32 261, ; 665
	i32 193, ; 666
	i32 247 ; 667
], align 4

@marshal_methods_number_of_classes = dso_local local_unnamed_addr constant i32 0, align 4

@marshal_methods_class_cache = dso_local local_unnamed_addr global [0 x %struct.MarshalMethodsManagedClass] zeroinitializer, align 4

; Names of classes in which marshal methods reside
@mm_class_names = dso_local local_unnamed_addr constant [0 x ptr] zeroinitializer, align 4

@mm_method_names = dso_local local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		ptr @.MarshalMethodName.0_name; char* name
	} ; 0
], align 8

; get_function_pointer (uint32_t mono_image_index, uint32_t class_index, uint32_t method_token, void*& target_ptr)
@get_function_pointer = internal dso_local unnamed_addr global ptr null, align 4

; Functions

; Function attributes: "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" uwtable willreturn
define void @xamarin_app_init(ptr nocapture noundef readnone %env, ptr noundef %fn) local_unnamed_addr #0
{
	%fnIsNull = icmp eq ptr %fn, null
	br i1 %fnIsNull, label %1, label %2

1: ; preds = %0
	%putsResult = call noundef i32 @puts(ptr @.str.0)
	call void @abort()
	unreachable 

2: ; preds = %1, %0
	store ptr %fn, ptr @get_function_pointer, align 4, !tbaa !3
	ret void
}

; Strings
@.str.0 = private unnamed_addr constant [40 x i8] c"get_function_pointer MUST be specified\0A\00", align 1

;MarshalMethodName
@.MarshalMethodName.0_name = private unnamed_addr constant [1 x i8] c"\00", align 1

; External functions

; Function attributes: noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8"
declare void @abort() local_unnamed_addr #2

; Function attributes: nofree nounwind
declare noundef i32 @puts(ptr noundef) local_unnamed_addr #1
attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-thumb-mode,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" uwtable willreturn }
attributes #1 = { nofree nounwind }
attributes #2 = { noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-thumb-mode,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" }

; Metadata
!llvm.module.flags = !{!0, !1, !7}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!llvm.ident = !{!2}
!2 = !{!"Xamarin.Android remotes/origin/release/8.0.1xx @ f1b7113872c8db3dfee70d11925e81bb752dc8d0"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
!7 = !{i32 1, !"min_enum_size", i32 4}
