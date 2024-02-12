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

@assembly_image_cache = dso_local local_unnamed_addr global [339 x ptr] zeroinitializer, align 4

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [672 x i32] [
	i32 2616222, ; 0: System.Net.NetworkInformation.dll => 0x27eb9e => 68
	i32 10166715, ; 1: System.Net.NameResolution.dll => 0x9b21bb => 67
	i32 15721112, ; 2: System.Runtime.Intrinsics.dll => 0xefe298 => 108
	i32 32687329, ; 3: Xamarin.AndroidX.Lifecycle.Runtime => 0x1f2c4e1 => 259
	i32 34715100, ; 4: Xamarin.Google.Guava.ListenableFuture.dll => 0x211b5dc => 293
	i32 34839235, ; 5: System.IO.FileSystem.DriveInfo => 0x2139ac3 => 48
	i32 38948123, ; 6: ar\Microsoft.Maui.Controls.resources.dll => 0x2524d1b => 301
	i32 39109920, ; 7: Newtonsoft.Json.dll => 0x254c520 => 215
	i32 39485524, ; 8: System.Net.WebSockets.dll => 0x25a8054 => 80
	i32 42244203, ; 9: he\Microsoft.Maui.Controls.resources.dll => 0x284986b => 310
	i32 42639949, ; 10: System.Threading.Thread => 0x28aa24d => 145
	i32 66541672, ; 11: System.Diagnostics.StackTrace => 0x3f75868 => 30
	i32 67008169, ; 12: zh-Hant\Microsoft.Maui.Controls.resources => 0x3fe76a9 => 334
	i32 68219467, ; 13: System.Security.Cryptography.Primitives => 0x410f24b => 124
	i32 72070932, ; 14: Microsoft.Maui.Graphics.dll => 0x44bb714 => 214
	i32 82292897, ; 15: System.Runtime.CompilerServices.VisualC.dll => 0x4e7b0a1 => 102
	i32 83839681, ; 16: ms\Microsoft.Maui.Controls.resources.dll => 0x4ff4ac1 => 318
	i32 101534019, ; 17: Xamarin.AndroidX.SlidingPaneLayout => 0x60d4943 => 277
	i32 117431740, ; 18: System.Runtime.InteropServices => 0x6ffddbc => 107
	i32 120558881, ; 19: Xamarin.AndroidX.SlidingPaneLayout.dll => 0x72f9521 => 277
	i32 122350210, ; 20: System.Threading.Channels.dll => 0x74aea82 => 139
	i32 134690465, ; 21: Xamarin.Kotlin.StdLib.Jdk7.dll => 0x80736a1 => 297
	i32 136584136, ; 22: zh-Hans\Microsoft.Maui.Controls.resources.dll => 0x8241bc8 => 333
	i32 140062828, ; 23: sk\Microsoft.Maui.Controls.resources.dll => 0x859306c => 326
	i32 142721839, ; 24: System.Net.WebHeaderCollection => 0x881c32f => 77
	i32 149972175, ; 25: System.Security.Cryptography.Primitives.dll => 0x8f064cf => 124
	i32 159306688, ; 26: System.ComponentModel.Annotations => 0x97ed3c0 => 13
	i32 165246403, ; 27: Xamarin.AndroidX.Collection.dll => 0x9d975c3 => 233
	i32 176265551, ; 28: System.ServiceProcess => 0xa81994f => 132
	i32 182336117, ; 29: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0xade3a75 => 279
	i32 184328833, ; 30: System.ValueTuple.dll => 0xafca281 => 151
	i32 205061960, ; 31: System.ComponentModel => 0xc38ff48 => 18
	i32 209399409, ; 32: Xamarin.AndroidX.Browser.dll => 0xc7b2e71 => 231
	i32 220171995, ; 33: System.Diagnostics.Debug => 0xd1f8edb => 26
	i32 230216969, ; 34: Xamarin.AndroidX.Legacy.Support.Core.Utils.dll => 0xdb8d509 => 253
	i32 230752869, ; 35: Microsoft.CSharp.dll => 0xdc10265 => 1
	i32 231409092, ; 36: System.Linq.Parallel => 0xdcb05c4 => 59
	i32 231814094, ; 37: System.Globalization => 0xdd133ce => 42
	i32 244348769, ; 38: Microsoft.AspNetCore.Components.Authorization => 0xe907761 => 179
	i32 246610117, ; 39: System.Reflection.Emit.Lightweight => 0xeb2f8c5 => 91
	i32 254259026, ; 40: Microsoft.AspNetCore.Components.dll => 0xf27af52 => 178
	i32 261689757, ; 41: Xamarin.AndroidX.ConstraintLayout.dll => 0xf99119d => 236
	i32 276479776, ; 42: System.Threading.Timer.dll => 0x107abf20 => 147
	i32 278686392, ; 43: Xamarin.AndroidX.Lifecycle.LiveData.dll => 0x109c6ab8 => 255
	i32 280482487, ; 44: Xamarin.AndroidX.Interpolator => 0x10b7d2b7 => 252
	i32 291076382, ; 45: System.IO.Pipes.AccessControl.dll => 0x1159791e => 54
	i32 298918909, ; 46: System.Net.Ping.dll => 0x11d123fd => 69
	i32 317674968, ; 47: vi\Microsoft.Maui.Controls.resources => 0x12ef55d8 => 331
	i32 318968648, ; 48: Xamarin.AndroidX.Activity.dll => 0x13031348 => 222
	i32 321597661, ; 49: System.Numerics => 0x132b30dd => 83
	i32 321963286, ; 50: fr\Microsoft.Maui.Controls.resources.dll => 0x1330c516 => 309
	i32 342366114, ; 51: Xamarin.AndroidX.Lifecycle.Common => 0x146817a2 => 254
	i32 360082299, ; 52: System.ServiceModel.Web => 0x15766b7b => 131
	i32 367780167, ; 53: System.IO.Pipes => 0x15ebe147 => 55
	i32 374914964, ; 54: System.Transactions.Local => 0x1658bf94 => 149
	i32 375677976, ; 55: System.Net.ServicePoint.dll => 0x16646418 => 74
	i32 379916513, ; 56: System.Threading.Thread.dll => 0x16a510e1 => 145
	i32 385762202, ; 57: System.Memory.dll => 0x16fe439a => 62
	i32 392610295, ; 58: System.Threading.ThreadPool.dll => 0x1766c1f7 => 146
	i32 395744057, ; 59: _Microsoft.Android.Resource.Designer => 0x17969339 => 335
	i32 403441872, ; 60: WindowsBase => 0x180c08d0 => 165
	i32 409257351, ; 61: tr\Microsoft.Maui.Controls.resources.dll => 0x1864c587 => 329
	i32 441335492, ; 62: Xamarin.AndroidX.ConstraintLayout.Core => 0x1a4e3ec4 => 237
	i32 442565967, ; 63: System.Collections => 0x1a61054f => 12
	i32 450948140, ; 64: Xamarin.AndroidX.Fragment.dll => 0x1ae0ec2c => 250
	i32 451504562, ; 65: System.Security.Cryptography.X509Certificates => 0x1ae969b2 => 125
	i32 456227837, ; 66: System.Web.HttpUtility.dll => 0x1b317bfd => 152
	i32 459347974, ; 67: System.Runtime.Serialization.Primitives.dll => 0x1b611806 => 113
	i32 465846621, ; 68: mscorlib => 0x1bc4415d => 166
	i32 469710990, ; 69: System.dll => 0x1bff388e => 164
	i32 476646585, ; 70: Xamarin.AndroidX.Interpolator.dll => 0x1c690cb9 => 252
	i32 485463106, ; 71: Microsoft.IdentityModel.Abstractions => 0x1cef9442 => 204
	i32 486930444, ; 72: Xamarin.AndroidX.LocalBroadcastManager.dll => 0x1d05f80c => 265
	i32 489220957, ; 73: es\Microsoft.Maui.Controls.resources.dll => 0x1d28eb5d => 307
	i32 498788369, ; 74: System.ObjectModel => 0x1dbae811 => 84
	i32 513247710, ; 75: Microsoft.Extensions.Primitives.dll => 0x1e9789de => 202
	i32 526420162, ; 76: System.Transactions.dll => 0x1f6088c2 => 150
	i32 527452488, ; 77: Xamarin.Kotlin.StdLib.Jdk7 => 0x1f704948 => 297
	i32 530272170, ; 78: System.Linq.Queryable => 0x1f9b4faa => 60
	i32 538707440, ; 79: th\Microsoft.Maui.Controls.resources.dll => 0x201c05f0 => 328
	i32 539058512, ; 80: Microsoft.Extensions.Logging => 0x20216150 => 198
	i32 540030774, ; 81: System.IO.FileSystem.dll => 0x20303736 => 51
	i32 545304856, ; 82: System.Runtime.Extensions => 0x2080b118 => 103
	i32 546455878, ; 83: System.Runtime.Serialization.Xml => 0x20924146 => 114
	i32 549171840, ; 84: System.Globalization.Calendars => 0x20bbb280 => 40
	i32 557405415, ; 85: Jsr305Binding => 0x213954e7 => 290
	i32 569601784, ; 86: Xamarin.AndroidX.Window.Extensions.Core.Core => 0x21f36ef8 => 288
	i32 571435654, ; 87: Microsoft.Extensions.FileProviders.Embedded.dll => 0x220f6a86 => 195
	i32 577335427, ; 88: System.Security.Cryptography.Cng => 0x22697083 => 120
	i32 597488923, ; 89: CommunityToolkit.Maui => 0x239cf51b => 174
	i32 601371474, ; 90: System.IO.IsolatedStorage.dll => 0x23d83352 => 52
	i32 605376203, ; 91: System.IO.Compression.FileSystem => 0x24154ecb => 44
	i32 613668793, ; 92: System.Security.Cryptography.Algorithms => 0x2493d7b9 => 119
	i32 627609679, ; 93: Xamarin.AndroidX.CustomView => 0x2568904f => 242
	i32 627931235, ; 94: nl\Microsoft.Maui.Controls.resources => 0x256d7863 => 320
	i32 639843206, ; 95: Xamarin.AndroidX.Emoji2.ViewsHelper.dll => 0x26233b86 => 248
	i32 643868501, ; 96: System.Net => 0x2660a755 => 81
	i32 662205335, ; 97: System.Text.Encodings.Web.dll => 0x27787397 => 136
	i32 663517072, ; 98: Xamarin.AndroidX.VersionedParcelable => 0x278c7790 => 284
	i32 666292255, ; 99: Xamarin.AndroidX.Arch.Core.Common.dll => 0x27b6d01f => 229
	i32 672442732, ; 100: System.Collections.Concurrent => 0x2814a96c => 8
	i32 683518922, ; 101: System.Net.Security => 0x28bdabca => 73
	i32 690569205, ; 102: System.Xml.Linq.dll => 0x29293ff5 => 155
	i32 691348768, ; 103: Xamarin.KotlinX.Coroutines.Android.dll => 0x29352520 => 299
	i32 693804605, ; 104: System.Windows => 0x295a9e3d => 154
	i32 699345723, ; 105: System.Reflection.Emit => 0x29af2b3b => 92
	i32 700284507, ; 106: Xamarin.Jetbrains.Annotations => 0x29bd7e5b => 294
	i32 700358131, ; 107: System.IO.Compression.ZipFile => 0x29be9df3 => 45
	i32 720511267, ; 108: Xamarin.Kotlin.StdLib.Jdk8 => 0x2af22123 => 298
	i32 722857257, ; 109: System.Runtime.Loader.dll => 0x2b15ed29 => 109
	i32 735137430, ; 110: System.Security.SecureString.dll => 0x2bd14e96 => 129
	i32 752232764, ; 111: System.Diagnostics.Contracts.dll => 0x2cd6293c => 25
	i32 755313932, ; 112: Xamarin.Android.Glide.Annotations.dll => 0x2d052d0c => 219
	i32 759454413, ; 113: System.Net.Requests => 0x2d445acd => 72
	i32 762598435, ; 114: System.IO.Pipes.dll => 0x2d745423 => 55
	i32 775507847, ; 115: System.IO.Compression => 0x2e394f87 => 46
	i32 777317022, ; 116: sk\Microsoft.Maui.Controls.resources => 0x2e54ea9e => 326
	i32 789151979, ; 117: Microsoft.Extensions.Options => 0x2f0980eb => 201
	i32 790371945, ; 118: Xamarin.AndroidX.CustomView.PoolingContainer.dll => 0x2f1c1e69 => 243
	i32 804008546, ; 119: Microsoft.AspNetCore.Components.WebView.Maui => 0x2fec3262 => 184
	i32 804715423, ; 120: System.Data.Common => 0x2ff6fb9f => 22
	i32 807930345, ; 121: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx.dll => 0x302809e9 => 257
	i32 823281589, ; 122: System.Private.Uri.dll => 0x311247b5 => 86
	i32 830298997, ; 123: System.IO.Compression.Brotli => 0x317d5b75 => 43
	i32 832635846, ; 124: System.Xml.XPath.dll => 0x31a103c6 => 160
	i32 834051424, ; 125: System.Net.Quic => 0x31b69d60 => 71
	i32 843511501, ; 126: Xamarin.AndroidX.Print => 0x3246f6cd => 270
	i32 869139383, ; 127: hi\Microsoft.Maui.Controls.resources.dll => 0x33ce03b7 => 311
	i32 873119928, ; 128: Microsoft.VisualBasic => 0x340ac0b8 => 3
	i32 877678880, ; 129: System.Globalization.dll => 0x34505120 => 42
	i32 878954865, ; 130: System.Net.Http.Json => 0x3463c971 => 63
	i32 880668424, ; 131: ru\Microsoft.Maui.Controls.resources.dll => 0x347def08 => 325
	i32 904024072, ; 132: System.ComponentModel.Primitives.dll => 0x35e25008 => 16
	i32 911108515, ; 133: System.IO.MemoryMappedFiles.dll => 0x364e69a3 => 53
	i32 918734561, ; 134: pt-BR\Microsoft.Maui.Controls.resources.dll => 0x36c2c6e1 => 322
	i32 928116545, ; 135: Xamarin.Google.Guava.ListenableFuture => 0x3751ef41 => 293
	i32 952186615, ; 136: System.Runtime.InteropServices.JavaScript.dll => 0x38c136f7 => 105
	i32 955402788, ; 137: Newtonsoft.Json => 0x38f24a24 => 215
	i32 956575887, ; 138: Xamarin.Kotlin.StdLib.Jdk8.dll => 0x3904308f => 298
	i32 961460050, ; 139: it\Microsoft.Maui.Controls.resources.dll => 0x394eb752 => 315
	i32 966729478, ; 140: Xamarin.Google.Crypto.Tink.Android => 0x399f1f06 => 291
	i32 967690846, ; 141: Xamarin.AndroidX.Lifecycle.Common.dll => 0x39adca5e => 254
	i32 975236339, ; 142: System.Diagnostics.Tracing => 0x3a20ecf3 => 34
	i32 975874589, ; 143: System.Xml.XDocument => 0x3a2aaa1d => 158
	i32 986514023, ; 144: System.Private.DataContractSerialization.dll => 0x3acd0267 => 85
	i32 987214855, ; 145: System.Diagnostics.Tools => 0x3ad7b407 => 32
	i32 990727110, ; 146: ro\Microsoft.Maui.Controls.resources.dll => 0x3b0d4bc6 => 324
	i32 992768348, ; 147: System.Collections.dll => 0x3b2c715c => 12
	i32 994442037, ; 148: System.IO.FileSystem => 0x3b45fb35 => 51
	i32 999186168, ; 149: Microsoft.Extensions.FileSystemGlobbing.dll => 0x3b8e5ef8 => 197
	i32 1001831731, ; 150: System.IO.UnmanagedMemoryStream.dll => 0x3bb6bd33 => 56
	i32 1012816738, ; 151: Xamarin.AndroidX.SavedState.dll => 0x3c5e5b62 => 274
	i32 1019214401, ; 152: System.Drawing => 0x3cbffa41 => 36
	i32 1028951442, ; 153: Microsoft.Extensions.DependencyInjection.Abstractions => 0x3d548d92 => 192
	i32 1031528504, ; 154: Xamarin.Google.ErrorProne.Annotations.dll => 0x3d7be038 => 292
	i32 1035644815, ; 155: Xamarin.AndroidX.AppCompat => 0x3dbaaf8f => 227
	i32 1036536393, ; 156: System.Drawing.Primitives.dll => 0x3dc84a49 => 35
	i32 1043950537, ; 157: de\Microsoft.Maui.Controls.resources.dll => 0x3e396bc9 => 305
	i32 1044663988, ; 158: System.Linq.Expressions.dll => 0x3e444eb4 => 58
	i32 1052210849, ; 159: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x3eb776a1 => 261
	i32 1067306892, ; 160: GoogleGson => 0x3f9dcf8c => 176
	i32 1082857460, ; 161: System.ComponentModel.TypeConverter => 0x408b17f4 => 17
	i32 1084122840, ; 162: Xamarin.Kotlin.StdLib => 0x409e66d8 => 295
	i32 1098259244, ; 163: System => 0x41761b2c => 164
	i32 1106973742, ; 164: Microsoft.Extensions.Configuration.FileExtensions.dll => 0x41fb142e => 189
	i32 1108272742, ; 165: sv\Microsoft.Maui.Controls.resources.dll => 0x420ee666 => 327
	i32 1117529484, ; 166: pl\Microsoft.Maui.Controls.resources.dll => 0x429c258c => 321
	i32 1118262833, ; 167: ko\Microsoft.Maui.Controls.resources => 0x42a75631 => 317
	i32 1121599056, ; 168: Xamarin.AndroidX.Lifecycle.Runtime.Ktx.dll => 0x42da3e50 => 260
	i32 1127624469, ; 169: Microsoft.Extensions.Logging.Debug => 0x43362f15 => 200
	i32 1149092582, ; 170: Xamarin.AndroidX.Window => 0x447dc2e6 => 287
	i32 1168523401, ; 171: pt\Microsoft.Maui.Controls.resources => 0x45a64089 => 323
	i32 1170634674, ; 172: System.Web.dll => 0x45c677b2 => 153
	i32 1173126369, ; 173: Microsoft.Extensions.FileProviders.Abstractions.dll => 0x45ec7ce1 => 193
	i32 1175144683, ; 174: Xamarin.AndroidX.VectorDrawable.Animated => 0x460b48eb => 283
	i32 1178241025, ; 175: Xamarin.AndroidX.Navigation.Runtime.dll => 0x463a8801 => 268
	i32 1204270330, ; 176: Xamarin.AndroidX.Arch.Core.Common => 0x47c7b4fa => 229
	i32 1208641965, ; 177: System.Diagnostics.Process => 0x480a69ad => 29
	i32 1219128291, ; 178: System.IO.IsolatedStorage => 0x48aa6be3 => 52
	i32 1228497194, ; 179: MsalAuthInMauiBlazor.dll => 0x4939612a => 0
	i32 1243150071, ; 180: Xamarin.AndroidX.Window.Extensions.Core.Core.dll => 0x4a18f6f7 => 288
	i32 1253011324, ; 181: Microsoft.Win32.Registry => 0x4aaf6f7c => 5
	i32 1260983243, ; 182: cs\Microsoft.Maui.Controls.resources => 0x4b2913cb => 303
	i32 1264511973, ; 183: Xamarin.AndroidX.Startup.StartupRuntime.dll => 0x4b5eebe5 => 278
	i32 1267360935, ; 184: Xamarin.AndroidX.VectorDrawable => 0x4b8a64a7 => 282
	i32 1273260888, ; 185: Xamarin.AndroidX.Collection.Ktx => 0x4be46b58 => 234
	i32 1275534314, ; 186: Xamarin.KotlinX.Coroutines.Android => 0x4c071bea => 299
	i32 1278448581, ; 187: Xamarin.AndroidX.Annotation.Jvm => 0x4c3393c5 => 226
	i32 1293217323, ; 188: Xamarin.AndroidX.DrawerLayout.dll => 0x4d14ee2b => 245
	i32 1308624726, ; 189: hr\Microsoft.Maui.Controls.resources.dll => 0x4e000756 => 312
	i32 1309188875, ; 190: System.Private.DataContractSerialization => 0x4e08a30b => 85
	i32 1322716291, ; 191: Xamarin.AndroidX.Window.dll => 0x4ed70c83 => 287
	i32 1324164729, ; 192: System.Linq => 0x4eed2679 => 61
	i32 1335329327, ; 193: System.Runtime.Serialization.Json.dll => 0x4f97822f => 112
	i32 1336711579, ; 194: zh-HK\Microsoft.Maui.Controls.resources.dll => 0x4fac999b => 332
	i32 1364015309, ; 195: System.IO => 0x514d38cd => 57
	i32 1373134921, ; 196: zh-Hans\Microsoft.Maui.Controls.resources => 0x51d86049 => 333
	i32 1376866003, ; 197: Xamarin.AndroidX.SavedState => 0x52114ed3 => 274
	i32 1379779777, ; 198: System.Resources.ResourceManager => 0x523dc4c1 => 99
	i32 1402170036, ; 199: System.Configuration.dll => 0x53936ab4 => 19
	i32 1406073936, ; 200: Xamarin.AndroidX.CoordinatorLayout => 0x53cefc50 => 238
	i32 1408764838, ; 201: System.Runtime.Serialization.Formatters.dll => 0x53f80ba6 => 111
	i32 1411638395, ; 202: System.Runtime.CompilerServices.Unsafe => 0x5423e47b => 101
	i32 1422545099, ; 203: System.Runtime.CompilerServices.VisualC => 0x54ca50cb => 102
	i32 1430672901, ; 204: ar\Microsoft.Maui.Controls.resources => 0x55465605 => 301
	i32 1434145427, ; 205: System.Runtime.Handles => 0x557b5293 => 104
	i32 1435222561, ; 206: Xamarin.Google.Crypto.Tink.Android.dll => 0x558bc221 => 291
	i32 1439761251, ; 207: System.Net.Quic.dll => 0x55d10363 => 71
	i32 1452070440, ; 208: System.Formats.Asn1.dll => 0x568cd628 => 38
	i32 1453312822, ; 209: System.Diagnostics.Tools.dll => 0x569fcb36 => 32
	i32 1454105418, ; 210: Microsoft.Extensions.FileProviders.Composite => 0x56abe34a => 194
	i32 1457743152, ; 211: System.Runtime.Extensions.dll => 0x56e36530 => 103
	i32 1458022317, ; 212: System.Net.Security.dll => 0x56e7a7ad => 73
	i32 1460893475, ; 213: System.IdentityModel.Tokens.Jwt => 0x57137723 => 216
	i32 1461004990, ; 214: es\Microsoft.Maui.Controls.resources => 0x57152abe => 307
	i32 1461234159, ; 215: System.Collections.Immutable.dll => 0x5718a9ef => 9
	i32 1461719063, ; 216: System.Security.Cryptography.OpenSsl => 0x57201017 => 123
	i32 1462112819, ; 217: System.IO.Compression.dll => 0x57261233 => 46
	i32 1469204771, ; 218: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x57924923 => 228
	i32 1470490898, ; 219: Microsoft.Extensions.Primitives => 0x57a5e912 => 202
	i32 1479771757, ; 220: System.Collections.Immutable => 0x5833866d => 9
	i32 1480492111, ; 221: System.IO.Compression.Brotli.dll => 0x583e844f => 43
	i32 1487239319, ; 222: Microsoft.Win32.Primitives => 0x58a57897 => 4
	i32 1490025113, ; 223: Xamarin.AndroidX.SavedState.SavedState.Ktx.dll => 0x58cffa99 => 275
	i32 1498168481, ; 224: Microsoft.IdentityModel.JsonWebTokens.dll => 0x594c3ca1 => 205
	i32 1521091094, ; 225: Microsoft.Extensions.FileSystemGlobbing => 0x5aaa0216 => 197
	i32 1526286932, ; 226: vi\Microsoft.Maui.Controls.resources.dll => 0x5af94a54 => 331
	i32 1536373174, ; 227: System.Diagnostics.TextWriterTraceListener => 0x5b9331b6 => 31
	i32 1543031311, ; 228: System.Text.RegularExpressions.dll => 0x5bf8ca0f => 138
	i32 1543355203, ; 229: System.Reflection.Emit.dll => 0x5bfdbb43 => 92
	i32 1546581739, ; 230: Microsoft.AspNetCore.Components.WebView.Maui.dll => 0x5c2ef6eb => 184
	i32 1550322496, ; 231: System.Reflection.Extensions.dll => 0x5c680b40 => 93
	i32 1565862583, ; 232: System.IO.FileSystem.Primitives => 0x5d552ab7 => 49
	i32 1566207040, ; 233: System.Threading.Tasks.Dataflow.dll => 0x5d5a6c40 => 141
	i32 1573704789, ; 234: System.Runtime.Serialization.Json => 0x5dccd455 => 112
	i32 1580037396, ; 235: System.Threading.Overlapped => 0x5e2d7514 => 140
	i32 1582372066, ; 236: Xamarin.AndroidX.DocumentFile.dll => 0x5e5114e2 => 244
	i32 1592978981, ; 237: System.Runtime.Serialization.dll => 0x5ef2ee25 => 115
	i32 1597949149, ; 238: Xamarin.Google.ErrorProne.Annotations => 0x5f3ec4dd => 292
	i32 1601112923, ; 239: System.Xml.Serialization => 0x5f6f0b5b => 157
	i32 1604827217, ; 240: System.Net.WebClient => 0x5fa7b851 => 76
	i32 1618516317, ; 241: System.Net.WebSockets.Client.dll => 0x6078995d => 79
	i32 1622152042, ; 242: Xamarin.AndroidX.Loader.dll => 0x60b0136a => 264
	i32 1622358360, ; 243: System.Dynamic.Runtime => 0x60b33958 => 37
	i32 1624863272, ; 244: Xamarin.AndroidX.ViewPager2 => 0x60d97228 => 286
	i32 1632842087, ; 245: Microsoft.Extensions.Configuration.Json => 0x61533167 => 190
	i32 1634654947, ; 246: CommunityToolkit.Maui.Core.dll => 0x616edae3 => 175
	i32 1635184631, ; 247: Xamarin.AndroidX.Emoji2.ViewsHelper => 0x6176eff7 => 248
	i32 1636350590, ; 248: Xamarin.AndroidX.CursorAdapter => 0x6188ba7e => 241
	i32 1639515021, ; 249: System.Net.Http.dll => 0x61b9038d => 64
	i32 1639986890, ; 250: System.Text.RegularExpressions => 0x61c036ca => 138
	i32 1641389582, ; 251: System.ComponentModel.EventBasedAsync.dll => 0x61d59e0e => 15
	i32 1654881142, ; 252: Microsoft.AspNetCore.Components.WebView => 0x62a37b76 => 183
	i32 1657153582, ; 253: System.Runtime => 0x62c6282e => 116
	i32 1658241508, ; 254: Xamarin.AndroidX.Tracing.Tracing.dll => 0x62d6c1e4 => 280
	i32 1658251792, ; 255: Xamarin.Google.Android.Material.dll => 0x62d6ea10 => 289
	i32 1670060433, ; 256: Xamarin.AndroidX.ConstraintLayout => 0x638b1991 => 236
	i32 1675553242, ; 257: System.IO.FileSystem.DriveInfo.dll => 0x63dee9da => 48
	i32 1677501392, ; 258: System.Net.Primitives.dll => 0x63fca3d0 => 70
	i32 1678508291, ; 259: System.Net.WebSockets => 0x640c0103 => 80
	i32 1679018464, ; 260: Blazored.LocalStorage => 0x6413c9e0 => 173
	i32 1679769178, ; 261: System.Security.Cryptography => 0x641f3e5a => 126
	i32 1691477237, ; 262: System.Reflection.Metadata => 0x64d1e4f5 => 94
	i32 1696967625, ; 263: System.Security.Cryptography.Csp => 0x6525abc9 => 121
	i32 1698840827, ; 264: Xamarin.Kotlin.StdLib.Common => 0x654240fb => 296
	i32 1701541528, ; 265: System.Diagnostics.Debug.dll => 0x656b7698 => 26
	i32 1720223769, ; 266: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx => 0x66888819 => 257
	i32 1726116996, ; 267: System.Reflection.dll => 0x66e27484 => 97
	i32 1728033016, ; 268: System.Diagnostics.FileVersionInfo.dll => 0x66ffb0f8 => 28
	i32 1729485958, ; 269: Xamarin.AndroidX.CardView.dll => 0x6715dc86 => 232
	i32 1743415430, ; 270: ca\Microsoft.Maui.Controls.resources => 0x67ea6886 => 302
	i32 1744735666, ; 271: System.Transactions.Local.dll => 0x67fe8db2 => 149
	i32 1746115085, ; 272: System.IO.Pipelines.dll => 0x68139a0d => 217
	i32 1746316138, ; 273: Mono.Android.Export => 0x6816ab6a => 169
	i32 1750313021, ; 274: Microsoft.Win32.Primitives.dll => 0x6853a83d => 4
	i32 1758240030, ; 275: System.Resources.Reader.dll => 0x68cc9d1e => 98
	i32 1760259689, ; 276: Microsoft.AspNetCore.Components.Web.dll => 0x68eb6e69 => 181
	i32 1763938596, ; 277: System.Diagnostics.TraceSource.dll => 0x69239124 => 33
	i32 1765942094, ; 278: System.Reflection.Extensions => 0x6942234e => 93
	i32 1766324549, ; 279: Xamarin.AndroidX.SwipeRefreshLayout => 0x6947f945 => 279
	i32 1770582343, ; 280: Microsoft.Extensions.Logging.dll => 0x6988f147 => 198
	i32 1776026572, ; 281: System.Core.dll => 0x69dc03cc => 21
	i32 1777075843, ; 282: System.Globalization.Extensions.dll => 0x69ec0683 => 41
	i32 1780572499, ; 283: Mono.Android.Runtime.dll => 0x6a216153 => 170
	i32 1782862114, ; 284: ms\Microsoft.Maui.Controls.resources => 0x6a445122 => 318
	i32 1788241197, ; 285: Xamarin.AndroidX.Fragment => 0x6a96652d => 250
	i32 1793755602, ; 286: he\Microsoft.Maui.Controls.resources => 0x6aea89d2 => 310
	i32 1794500907, ; 287: Microsoft.Identity.Client.dll => 0x6af5e92b => 203
	i32 1808609942, ; 288: Xamarin.AndroidX.Loader => 0x6bcd3296 => 264
	i32 1813058853, ; 289: Xamarin.Kotlin.StdLib.dll => 0x6c111525 => 295
	i32 1813201214, ; 290: Xamarin.Google.Android.Material => 0x6c13413e => 289
	i32 1818569960, ; 291: Xamarin.AndroidX.Navigation.UI.dll => 0x6c652ce8 => 269
	i32 1818787751, ; 292: Microsoft.VisualBasic.Core => 0x6c687fa7 => 2
	i32 1824175904, ; 293: System.Text.Encoding.Extensions => 0x6cbab720 => 134
	i32 1824722060, ; 294: System.Runtime.Serialization.Formatters => 0x6cc30c8c => 111
	i32 1827745125, ; 295: Microsoft.AspNetCore.Components.WebAssembly.Authentication => 0x6cf12d65 => 182
	i32 1828688058, ; 296: Microsoft.Extensions.Logging.Abstractions.dll => 0x6cff90ba => 199
	i32 1847515442, ; 297: Xamarin.Android.Glide.Annotations => 0x6e1ed932 => 219
	i32 1853025655, ; 298: sv\Microsoft.Maui.Controls.resources => 0x6e72ed77 => 327
	i32 1858542181, ; 299: System.Linq.Expressions => 0x6ec71a65 => 58
	i32 1870277092, ; 300: System.Reflection.Primitives => 0x6f7a29e4 => 95
	i32 1875935024, ; 301: fr\Microsoft.Maui.Controls.resources => 0x6fd07f30 => 309
	i32 1879696579, ; 302: System.Formats.Tar.dll => 0x7009e4c3 => 39
	i32 1885316902, ; 303: Xamarin.AndroidX.Arch.Core.Runtime.dll => 0x705fa726 => 230
	i32 1888955245, ; 304: System.Diagnostics.Contracts => 0x70972b6d => 25
	i32 1889954781, ; 305: System.Reflection.Metadata.dll => 0x70a66bdd => 94
	i32 1893218855, ; 306: cs\Microsoft.Maui.Controls.resources.dll => 0x70d83a27 => 303
	i32 1898237753, ; 307: System.Reflection.DispatchProxy => 0x7124cf39 => 89
	i32 1900610850, ; 308: System.Resources.ResourceManager.dll => 0x71490522 => 99
	i32 1910275211, ; 309: System.Collections.NonGeneric.dll => 0x71dc7c8b => 10
	i32 1939592360, ; 310: System.Private.Xml.Linq => 0x739bd4a8 => 87
	i32 1953182387, ; 311: id\Microsoft.Maui.Controls.resources.dll => 0x746b32b3 => 314
	i32 1956758971, ; 312: System.Resources.Writer => 0x74a1c5bb => 100
	i32 1961813231, ; 313: Xamarin.AndroidX.Security.SecurityCrypto.dll => 0x74eee4ef => 276
	i32 1968388702, ; 314: Microsoft.Extensions.Configuration.dll => 0x75533a5e => 186
	i32 1983156543, ; 315: Xamarin.Kotlin.StdLib.Common.dll => 0x7634913f => 296
	i32 1985761444, ; 316: Xamarin.Android.Glide.GifDecoder => 0x765c50a4 => 221
	i32 1986222447, ; 317: Microsoft.IdentityModel.Tokens.dll => 0x7663596f => 207
	i32 2003115576, ; 318: el\Microsoft.Maui.Controls.resources => 0x77651e38 => 306
	i32 2011961780, ; 319: System.Buffers.dll => 0x77ec19b4 => 7
	i32 2019465201, ; 320: Xamarin.AndroidX.Lifecycle.ViewModel => 0x785e97f1 => 261
	i32 2031763787, ; 321: Xamarin.Android.Glide => 0x791a414b => 218
	i32 2045470958, ; 322: System.Private.Xml => 0x79eb68ee => 88
	i32 2048278909, ; 323: Microsoft.Extensions.Configuration.Binder.dll => 0x7a16417d => 188
	i32 2055257422, ; 324: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x7a80bd4e => 256
	i32 2060060697, ; 325: System.Windows.dll => 0x7aca0819 => 154
	i32 2066184531, ; 326: de\Microsoft.Maui.Controls.resources => 0x7b277953 => 305
	i32 2070888862, ; 327: System.Diagnostics.TraceSource => 0x7b6f419e => 33
	i32 2072397586, ; 328: Microsoft.Extensions.FileProviders.Physical => 0x7b864712 => 196
	i32 2079903147, ; 329: System.Runtime.dll => 0x7bf8cdab => 116
	i32 2090596640, ; 330: System.Numerics.Vectors => 0x7c9bf920 => 82
	i32 2127167465, ; 331: System.Console => 0x7ec9ffe9 => 20
	i32 2142473426, ; 332: System.Collections.Specialized => 0x7fb38cd2 => 11
	i32 2143790110, ; 333: System.Xml.XmlSerializer.dll => 0x7fc7a41e => 162
	i32 2146852085, ; 334: Microsoft.VisualBasic.dll => 0x7ff65cf5 => 3
	i32 2159891885, ; 335: Microsoft.Maui => 0x80bd55ad => 212
	i32 2169148018, ; 336: hu\Microsoft.Maui.Controls.resources => 0x814a9272 => 313
	i32 2181898931, ; 337: Microsoft.Extensions.Options.dll => 0x820d22b3 => 201
	i32 2192057212, ; 338: Microsoft.Extensions.Logging.Abstractions => 0x82a8237c => 199
	i32 2192166651, ; 339: Microsoft.AspNetCore.Components.Authorization.dll => 0x82a9cefb => 179
	i32 2193016926, ; 340: System.ObjectModel.dll => 0x82b6c85e => 84
	i32 2201107256, ; 341: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 0x83323b38 => 300
	i32 2201231467, ; 342: System.Net.Http => 0x8334206b => 64
	i32 2207618523, ; 343: it\Microsoft.Maui.Controls.resources => 0x839595db => 315
	i32 2217644978, ; 344: Xamarin.AndroidX.VectorDrawable.Animated.dll => 0x842e93b2 => 283
	i32 2222056684, ; 345: System.Threading.Tasks.Parallel => 0x8471e4ec => 143
	i32 2244775296, ; 346: Xamarin.AndroidX.LocalBroadcastManager => 0x85cc8d80 => 265
	i32 2252106437, ; 347: System.Xml.Serialization.dll => 0x863c6ac5 => 157
	i32 2256313426, ; 348: System.Globalization.Extensions => 0x867c9c52 => 41
	i32 2265110946, ; 349: System.Security.AccessControl.dll => 0x8702d9a2 => 117
	i32 2266799131, ; 350: Microsoft.Extensions.Configuration.Abstractions => 0x871c9c1b => 187
	i32 2267999099, ; 351: Xamarin.Android.Glide.DiskLruCache.dll => 0x872eeb7b => 220
	i32 2279755925, ; 352: Xamarin.AndroidX.RecyclerView.dll => 0x87e25095 => 272
	i32 2293034957, ; 353: System.ServiceModel.Web.dll => 0x88acefcd => 131
	i32 2295906218, ; 354: System.Net.Sockets => 0x88d8bfaa => 75
	i32 2298471582, ; 355: System.Net.Mail => 0x88ffe49e => 66
	i32 2303942373, ; 356: nb\Microsoft.Maui.Controls.resources => 0x89535ee5 => 319
	i32 2305521784, ; 357: System.Private.CoreLib.dll => 0x896b7878 => 172
	i32 2311968808, ; 358: Blazored.LocalStorage.dll => 0x89cdd828 => 173
	i32 2315684594, ; 359: Xamarin.AndroidX.Annotation.dll => 0x8a068af2 => 224
	i32 2320631194, ; 360: System.Threading.Tasks.Parallel.dll => 0x8a52059a => 143
	i32 2340441535, ; 361: System.Runtime.InteropServices.RuntimeInformation.dll => 0x8b804dbf => 106
	i32 2344264397, ; 362: System.ValueTuple => 0x8bbaa2cd => 151
	i32 2353062107, ; 363: System.Net.Primitives => 0x8c40e0db => 70
	i32 2366048013, ; 364: hu\Microsoft.Maui.Controls.resources.dll => 0x8d07070d => 313
	i32 2368005991, ; 365: System.Xml.ReaderWriter.dll => 0x8d24e767 => 156
	i32 2369706906, ; 366: Microsoft.IdentityModel.Logging => 0x8d3edb9a => 206
	i32 2371007202, ; 367: Microsoft.Extensions.Configuration => 0x8d52b2e2 => 186
	i32 2378619854, ; 368: System.Security.Cryptography.Csp.dll => 0x8dc6dbce => 121
	i32 2383496789, ; 369: System.Security.Principal.Windows.dll => 0x8e114655 => 127
	i32 2395872292, ; 370: id\Microsoft.Maui.Controls.resources => 0x8ece1c24 => 314
	i32 2401565422, ; 371: System.Web.HttpUtility => 0x8f24faee => 152
	i32 2403452196, ; 372: Xamarin.AndroidX.Emoji2.dll => 0x8f41c524 => 247
	i32 2411328690, ; 373: Microsoft.AspNetCore.Components => 0x8fb9f4b2 => 178
	i32 2421380589, ; 374: System.Threading.Tasks.Dataflow => 0x905355ed => 141
	i32 2423080555, ; 375: Xamarin.AndroidX.Collection.Ktx.dll => 0x906d466b => 234
	i32 2427813419, ; 376: hi\Microsoft.Maui.Controls.resources => 0x90b57e2b => 311
	i32 2435356389, ; 377: System.Console.dll => 0x912896e5 => 20
	i32 2435904999, ; 378: System.ComponentModel.DataAnnotations.dll => 0x9130f5e7 => 14
	i32 2442556106, ; 379: Microsoft.JSInterop.dll => 0x919672ca => 208
	i32 2454642406, ; 380: System.Text.Encoding.dll => 0x924edee6 => 135
	i32 2458678730, ; 381: System.Net.Sockets.dll => 0x928c75ca => 75
	i32 2459001652, ; 382: System.Linq.Parallel.dll => 0x92916334 => 59
	i32 2465532216, ; 383: Xamarin.AndroidX.ConstraintLayout.Core.dll => 0x92f50938 => 237
	i32 2471841756, ; 384: netstandard.dll => 0x93554fdc => 167
	i32 2475788418, ; 385: Java.Interop.dll => 0x93918882 => 168
	i32 2480646305, ; 386: Microsoft.Maui.Controls => 0x93dba8a1 => 210
	i32 2483903535, ; 387: System.ComponentModel.EventBasedAsync => 0x940d5c2f => 15
	i32 2484371297, ; 388: System.Net.ServicePoint => 0x94147f61 => 74
	i32 2490993605, ; 389: System.AppContext.dll => 0x94798bc5 => 6
	i32 2501346920, ; 390: System.Data.DataSetExtensions => 0x95178668 => 23
	i32 2503351294, ; 391: ko\Microsoft.Maui.Controls.resources.dll => 0x95361bfe => 317
	i32 2505896520, ; 392: Xamarin.AndroidX.Lifecycle.Runtime.dll => 0x955cf248 => 259
	i32 2522472828, ; 393: Xamarin.Android.Glide.dll => 0x9659e17c => 218
	i32 2537015816, ; 394: Microsoft.AspNetCore.Authorization => 0x9737ca08 => 177
	i32 2538310050, ; 395: System.Reflection.Emit.Lightweight.dll => 0x974b89a2 => 91
	i32 2550873716, ; 396: hr\Microsoft.Maui.Controls.resources => 0x980b3e74 => 312
	i32 2562349572, ; 397: Microsoft.CSharp => 0x98ba5a04 => 1
	i32 2570120770, ; 398: System.Text.Encodings.Web => 0x9930ee42 => 136
	i32 2576534780, ; 399: ja\Microsoft.Maui.Controls.resources.dll => 0x9992ccfc => 316
	i32 2581783588, ; 400: Xamarin.AndroidX.Lifecycle.Runtime.Ktx => 0x99e2e424 => 260
	i32 2581819634, ; 401: Xamarin.AndroidX.VectorDrawable.dll => 0x99e370f2 => 282
	i32 2585220780, ; 402: System.Text.Encoding.Extensions.dll => 0x9a1756ac => 134
	i32 2585805581, ; 403: System.Net.Ping => 0x9a20430d => 69
	i32 2585813321, ; 404: Microsoft.AspNetCore.Components.Forms => 0x9a206149 => 180
	i32 2589602615, ; 405: System.Threading.ThreadPool => 0x9a5a3337 => 146
	i32 2592341985, ; 406: Microsoft.Extensions.FileProviders.Abstractions => 0x9a83ffe1 => 193
	i32 2593496499, ; 407: pl\Microsoft.Maui.Controls.resources => 0x9a959db3 => 321
	i32 2605712449, ; 408: Xamarin.KotlinX.Coroutines.Core.Jvm => 0x9b500441 => 300
	i32 2615233544, ; 409: Xamarin.AndroidX.Fragment.Ktx => 0x9be14c08 => 251
	i32 2616218305, ; 410: Microsoft.Extensions.Logging.Debug.dll => 0x9bf052c1 => 200
	i32 2617129537, ; 411: System.Private.Xml.dll => 0x9bfe3a41 => 88
	i32 2618712057, ; 412: System.Reflection.TypeExtensions.dll => 0x9c165ff9 => 96
	i32 2620871830, ; 413: Xamarin.AndroidX.CursorAdapter.dll => 0x9c375496 => 241
	i32 2624644809, ; 414: Xamarin.AndroidX.DynamicAnimation => 0x9c70e6c9 => 246
	i32 2626831493, ; 415: ja\Microsoft.Maui.Controls.resources => 0x9c924485 => 316
	i32 2627185994, ; 416: System.Diagnostics.TextWriterTraceListener.dll => 0x9c97ad4a => 31
	i32 2629843544, ; 417: System.IO.Compression.ZipFile.dll => 0x9cc03a58 => 45
	i32 2633051222, ; 418: Xamarin.AndroidX.Lifecycle.LiveData => 0x9cf12c56 => 255
	i32 2640290731, ; 419: Microsoft.IdentityModel.Logging.dll => 0x9d5fa3ab => 206
	i32 2657731189, ; 420: Microsoft.AspNetCore.Components.WebAssembly.Authentication.dll => 0x9e69c275 => 182
	i32 2663391936, ; 421: Xamarin.Android.Glide.DiskLruCache => 0x9ec022c0 => 220
	i32 2663698177, ; 422: System.Runtime.Loader => 0x9ec4cf01 => 109
	i32 2664396074, ; 423: System.Xml.XDocument.dll => 0x9ecf752a => 158
	i32 2665622720, ; 424: System.Drawing.Primitives => 0x9ee22cc0 => 35
	i32 2676780864, ; 425: System.Data.Common.dll => 0x9f8c6f40 => 22
	i32 2686887180, ; 426: System.Runtime.Serialization.Xml.dll => 0xa026a50c => 114
	i32 2692077919, ; 427: Microsoft.AspNetCore.Components.WebView.dll => 0xa075d95f => 183
	i32 2693849962, ; 428: System.IO.dll => 0xa090e36a => 57
	i32 2701096212, ; 429: Xamarin.AndroidX.Tracing.Tracing => 0xa0ff7514 => 280
	i32 2715334215, ; 430: System.Threading.Tasks.dll => 0xa1d8b647 => 144
	i32 2717744543, ; 431: System.Security.Claims => 0xa1fd7d9f => 118
	i32 2719963679, ; 432: System.Security.Cryptography.Cng.dll => 0xa21f5a1f => 120
	i32 2724373263, ; 433: System.Runtime.Numerics.dll => 0xa262a30f => 110
	i32 2732626843, ; 434: Xamarin.AndroidX.Activity => 0xa2e0939b => 222
	i32 2735172069, ; 435: System.Threading.Channels => 0xa30769e5 => 139
	i32 2735631878, ; 436: Microsoft.AspNetCore.Authorization.dll => 0xa30e6e06 => 177
	i32 2737747696, ; 437: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0xa32eb6f0 => 228
	i32 2740051746, ; 438: Microsoft.Identity.Client => 0xa351df22 => 203
	i32 2740698338, ; 439: ca\Microsoft.Maui.Controls.resources.dll => 0xa35bbce2 => 302
	i32 2740948882, ; 440: System.IO.Pipes.AccessControl => 0xa35f8f92 => 54
	i32 2748088231, ; 441: System.Runtime.InteropServices.JavaScript => 0xa3cc7fa7 => 105
	i32 2752995522, ; 442: pt-BR\Microsoft.Maui.Controls.resources => 0xa41760c2 => 322
	i32 2758225723, ; 443: Microsoft.Maui.Controls.Xaml => 0xa4672f3b => 211
	i32 2764765095, ; 444: Microsoft.Maui.dll => 0xa4caf7a7 => 212
	i32 2765824710, ; 445: System.Text.Encoding.CodePages.dll => 0xa4db22c6 => 133
	i32 2770495804, ; 446: Xamarin.Jetbrains.Annotations.dll => 0xa522693c => 294
	i32 2778768386, ; 447: Xamarin.AndroidX.ViewPager.dll => 0xa5a0a402 => 285
	i32 2779977773, ; 448: Xamarin.AndroidX.ResourceInspection.Annotation.dll => 0xa5b3182d => 273
	i32 2785988530, ; 449: th\Microsoft.Maui.Controls.resources => 0xa60ecfb2 => 328
	i32 2788224221, ; 450: Xamarin.AndroidX.Fragment.Ktx.dll => 0xa630ecdd => 251
	i32 2801831435, ; 451: Microsoft.Maui.Graphics => 0xa7008e0b => 214
	i32 2803228030, ; 452: System.Xml.XPath.XDocument.dll => 0xa715dd7e => 159
	i32 2810250172, ; 453: Xamarin.AndroidX.CoordinatorLayout.dll => 0xa78103bc => 238
	i32 2819470561, ; 454: System.Xml.dll => 0xa80db4e1 => 163
	i32 2821205001, ; 455: System.ServiceProcess.dll => 0xa8282c09 => 132
	i32 2821294376, ; 456: Xamarin.AndroidX.ResourceInspection.Annotation => 0xa8298928 => 273
	i32 2824502124, ; 457: System.Xml.XmlDocument => 0xa85a7b6c => 161
	i32 2833784645, ; 458: Microsoft.AspNetCore.Metadata => 0xa8e81f45 => 185
	i32 2838993487, ; 459: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx.dll => 0xa9379a4f => 262
	i32 2849599387, ; 460: System.Threading.Overlapped.dll => 0xa9d96f9b => 140
	i32 2853208004, ; 461: Xamarin.AndroidX.ViewPager => 0xaa107fc4 => 285
	i32 2855708567, ; 462: Xamarin.AndroidX.Transition => 0xaa36a797 => 281
	i32 2861098320, ; 463: Mono.Android.Export.dll => 0xaa88e550 => 169
	i32 2861189240, ; 464: Microsoft.Maui.Essentials => 0xaa8a4878 => 213
	i32 2868488919, ; 465: CommunityToolkit.Maui.Core => 0xaaf9aad7 => 175
	i32 2870099610, ; 466: Xamarin.AndroidX.Activity.Ktx.dll => 0xab123e9a => 223
	i32 2875164099, ; 467: Jsr305Binding.dll => 0xab5f85c3 => 290
	i32 2875220617, ; 468: System.Globalization.Calendars.dll => 0xab606289 => 40
	i32 2884993177, ; 469: Xamarin.AndroidX.ExifInterface => 0xabf58099 => 249
	i32 2887636118, ; 470: System.Net.dll => 0xac1dd496 => 81
	i32 2892341533, ; 471: Microsoft.AspNetCore.Components.Web => 0xac65a11d => 181
	i32 2899753641, ; 472: System.IO.UnmanagedMemoryStream => 0xacd6baa9 => 56
	i32 2900621748, ; 473: System.Dynamic.Runtime.dll => 0xace3f9b4 => 37
	i32 2901442782, ; 474: System.Reflection => 0xacf080de => 97
	i32 2905242038, ; 475: mscorlib.dll => 0xad2a79b6 => 166
	i32 2909740682, ; 476: System.Private.CoreLib => 0xad6f1e8a => 172
	i32 2911054922, ; 477: Microsoft.Extensions.FileProviders.Physical.dll => 0xad832c4a => 196
	i32 2916838712, ; 478: Xamarin.AndroidX.ViewPager2.dll => 0xaddb6d38 => 286
	i32 2919462931, ; 479: System.Numerics.Vectors.dll => 0xae037813 => 82
	i32 2921128767, ; 480: Xamarin.AndroidX.Annotation.Experimental.dll => 0xae1ce33f => 225
	i32 2936416060, ; 481: System.Resources.Reader => 0xaf06273c => 98
	i32 2940926066, ; 482: System.Diagnostics.StackTrace.dll => 0xaf4af872 => 30
	i32 2942453041, ; 483: System.Xml.XPath.XDocument => 0xaf624531 => 159
	i32 2959614098, ; 484: System.ComponentModel.dll => 0xb0682092 => 18
	i32 2968338931, ; 485: System.Security.Principal.Windows => 0xb0ed41f3 => 127
	i32 2972252294, ; 486: System.Security.Cryptography.Algorithms.dll => 0xb128f886 => 119
	i32 2978675010, ; 487: Xamarin.AndroidX.DrawerLayout => 0xb18af942 => 245
	i32 2987532451, ; 488: Xamarin.AndroidX.Security.SecurityCrypto => 0xb21220a3 => 276
	i32 2996846495, ; 489: Xamarin.AndroidX.Lifecycle.Process.dll => 0xb2a03f9f => 258
	i32 3016983068, ; 490: Xamarin.AndroidX.Startup.StartupRuntime => 0xb3d3821c => 278
	i32 3023353419, ; 491: WindowsBase.dll => 0xb434b64b => 165
	i32 3024354802, ; 492: Xamarin.AndroidX.Legacy.Support.Core.Utils => 0xb443fdf2 => 253
	i32 3038032645, ; 493: _Microsoft.Android.Resource.Designer.dll => 0xb514b305 => 335
	i32 3053864966, ; 494: fi\Microsoft.Maui.Controls.resources.dll => 0xb6064806 => 308
	i32 3056245963, ; 495: Xamarin.AndroidX.SavedState.SavedState.Ktx => 0xb62a9ccb => 275
	i32 3057625584, ; 496: Xamarin.AndroidX.Navigation.Common => 0xb63fa9f0 => 266
	i32 3059408633, ; 497: Mono.Android.Runtime => 0xb65adef9 => 170
	i32 3059793426, ; 498: System.ComponentModel.Primitives => 0xb660be12 => 16
	i32 3075834255, ; 499: System.Threading.Tasks => 0xb755818f => 144
	i32 3084678329, ; 500: Microsoft.IdentityModel.Tokens => 0xb7dc74b9 => 207
	i32 3090735792, ; 501: System.Security.Cryptography.X509Certificates.dll => 0xb838e2b0 => 125
	i32 3099732863, ; 502: System.Security.Claims.dll => 0xb8c22b7f => 118
	i32 3103242213, ; 503: MsalAuthInMauiBlazor => 0xb8f7b7e5 => 0
	i32 3103600923, ; 504: System.Formats.Asn1 => 0xb8fd311b => 38
	i32 3111772706, ; 505: System.Runtime.Serialization => 0xb979e222 => 115
	i32 3121463068, ; 506: System.IO.FileSystem.AccessControl.dll => 0xba0dbf1c => 47
	i32 3124832203, ; 507: System.Threading.Tasks.Extensions => 0xba4127cb => 142
	i32 3132293585, ; 508: System.Security.AccessControl => 0xbab301d1 => 117
	i32 3147165239, ; 509: System.Diagnostics.Tracing.dll => 0xbb95ee37 => 34
	i32 3148237826, ; 510: GoogleGson.dll => 0xbba64c02 => 176
	i32 3159123045, ; 511: System.Reflection.Primitives.dll => 0xbc4c6465 => 95
	i32 3160747431, ; 512: System.IO.MemoryMappedFiles => 0xbc652da7 => 53
	i32 3178803400, ; 513: Xamarin.AndroidX.Navigation.Fragment.dll => 0xbd78b0c8 => 267
	i32 3192346100, ; 514: System.Security.SecureString => 0xbe4755f4 => 129
	i32 3193515020, ; 515: System.Web => 0xbe592c0c => 153
	i32 3204380047, ; 516: System.Data.dll => 0xbefef58f => 24
	i32 3209718065, ; 517: System.Xml.XmlDocument.dll => 0xbf506931 => 161
	i32 3211777861, ; 518: Xamarin.AndroidX.DocumentFile => 0xbf6fd745 => 244
	i32 3220365878, ; 519: System.Threading => 0xbff2e236 => 148
	i32 3226221578, ; 520: System.Runtime.Handles.dll => 0xc04c3c0a => 104
	i32 3251039220, ; 521: System.Reflection.DispatchProxy.dll => 0xc1c6ebf4 => 89
	i32 3258312781, ; 522: Xamarin.AndroidX.CardView => 0xc235e84d => 232
	i32 3265493905, ; 523: System.Linq.Queryable.dll => 0xc2a37b91 => 60
	i32 3265893370, ; 524: System.Threading.Tasks.Extensions.dll => 0xc2a993fa => 142
	i32 3277815716, ; 525: System.Resources.Writer.dll => 0xc35f7fa4 => 100
	i32 3279906254, ; 526: Microsoft.Win32.Registry.dll => 0xc37f65ce => 5
	i32 3280506390, ; 527: System.ComponentModel.Annotations.dll => 0xc3888e16 => 13
	i32 3290767353, ; 528: System.Security.Cryptography.Encoding => 0xc4251ff9 => 122
	i32 3299363146, ; 529: System.Text.Encoding => 0xc4a8494a => 135
	i32 3303498502, ; 530: System.Diagnostics.FileVersionInfo => 0xc4e76306 => 28
	i32 3305363605, ; 531: fi\Microsoft.Maui.Controls.resources => 0xc503d895 => 308
	i32 3312457198, ; 532: Microsoft.IdentityModel.JsonWebTokens => 0xc57015ee => 205
	i32 3316684772, ; 533: System.Net.Requests.dll => 0xc5b097e4 => 72
	i32 3317135071, ; 534: Xamarin.AndroidX.CustomView.dll => 0xc5b776df => 242
	i32 3317144872, ; 535: System.Data => 0xc5b79d28 => 24
	i32 3340431453, ; 536: Xamarin.AndroidX.Arch.Core.Runtime => 0xc71af05d => 230
	i32 3345895724, ; 537: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller.dll => 0xc76e512c => 271
	i32 3346324047, ; 538: Xamarin.AndroidX.Navigation.Runtime => 0xc774da4f => 268
	i32 3357674450, ; 539: ru\Microsoft.Maui.Controls.resources => 0xc8220bd2 => 325
	i32 3358260929, ; 540: System.Text.Json => 0xc82afec1 => 137
	i32 3362336904, ; 541: Xamarin.AndroidX.Activity.Ktx => 0xc8693088 => 223
	i32 3362522851, ; 542: Xamarin.AndroidX.Core => 0xc86c06e3 => 239
	i32 3366347497, ; 543: Java.Interop => 0xc8a662e9 => 168
	i32 3374999561, ; 544: Xamarin.AndroidX.RecyclerView => 0xc92a6809 => 272
	i32 3381016424, ; 545: da\Microsoft.Maui.Controls.resources => 0xc9863768 => 304
	i32 3395150330, ; 546: System.Runtime.CompilerServices.Unsafe.dll => 0xca5de1fa => 101
	i32 3403906625, ; 547: System.Security.Cryptography.OpenSsl.dll => 0xcae37e41 => 123
	i32 3405233483, ; 548: Xamarin.AndroidX.CustomView.PoolingContainer => 0xcaf7bd4b => 243
	i32 3406629867, ; 549: Microsoft.Extensions.FileProviders.Composite.dll => 0xcb0d0beb => 194
	i32 3421170118, ; 550: Microsoft.Extensions.Configuration.Binder => 0xcbeae9c6 => 188
	i32 3428513518, ; 551: Microsoft.Extensions.DependencyInjection.dll => 0xcc5af6ee => 191
	i32 3429136800, ; 552: System.Xml => 0xcc6479a0 => 163
	i32 3430777524, ; 553: netstandard => 0xcc7d82b4 => 167
	i32 3441283291, ; 554: Xamarin.AndroidX.DynamicAnimation.dll => 0xcd1dd0db => 246
	i32 3445260447, ; 555: System.Formats.Tar => 0xcd5a809f => 39
	i32 3452344032, ; 556: Microsoft.Maui.Controls.Compatibility.dll => 0xcdc696e0 => 209
	i32 3458724246, ; 557: pt\Microsoft.Maui.Controls.resources.dll => 0xce27f196 => 323
	i32 3464190856, ; 558: Microsoft.AspNetCore.Components.Forms.dll => 0xce7b5b88 => 180
	i32 3471940407, ; 559: System.ComponentModel.TypeConverter.dll => 0xcef19b37 => 17
	i32 3476120550, ; 560: Mono.Android => 0xcf3163e6 => 171
	i32 3484440000, ; 561: ro\Microsoft.Maui.Controls.resources => 0xcfb055c0 => 324
	i32 3485117614, ; 562: System.Text.Json.dll => 0xcfbaacae => 137
	i32 3486566296, ; 563: System.Transactions => 0xcfd0c798 => 150
	i32 3493954962, ; 564: Xamarin.AndroidX.Concurrent.Futures.dll => 0xd0418592 => 235
	i32 3500000672, ; 565: Microsoft.JSInterop => 0xd09dc5a0 => 208
	i32 3509114376, ; 566: System.Xml.Linq => 0xd128d608 => 155
	i32 3515174580, ; 567: System.Security.dll => 0xd1854eb4 => 130
	i32 3530912306, ; 568: System.Configuration => 0xd2757232 => 19
	i32 3539954161, ; 569: System.Net.HttpListener => 0xd2ff69f1 => 65
	i32 3560100363, ; 570: System.Threading.Timer => 0xd432d20b => 147
	i32 3570554715, ; 571: System.IO.FileSystem.AccessControl => 0xd4d2575b => 47
	i32 3580758918, ; 572: zh-HK\Microsoft.Maui.Controls.resources => 0xd56e0b86 => 332
	i32 3592228221, ; 573: zh-Hant\Microsoft.Maui.Controls.resources.dll => 0xd61d0d7d => 334
	i32 3597029428, ; 574: Xamarin.Android.Glide.GifDecoder.dll => 0xd6665034 => 221
	i32 3598340787, ; 575: System.Net.WebSockets.Client => 0xd67a52b3 => 79
	i32 3608519521, ; 576: System.Linq.dll => 0xd715a361 => 61
	i32 3624195450, ; 577: System.Runtime.InteropServices.RuntimeInformation => 0xd804d57a => 106
	i32 3627220390, ; 578: Xamarin.AndroidX.Print.dll => 0xd832fda6 => 270
	i32 3633644679, ; 579: Xamarin.AndroidX.Annotation.Experimental => 0xd8950487 => 225
	i32 3638274909, ; 580: System.IO.FileSystem.Primitives.dll => 0xd8dbab5d => 49
	i32 3641597786, ; 581: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0xd90e5f5a => 256
	i32 3643446276, ; 582: tr\Microsoft.Maui.Controls.resources => 0xd92a9404 => 329
	i32 3643854240, ; 583: Xamarin.AndroidX.Navigation.Fragment => 0xd930cda0 => 267
	i32 3645089577, ; 584: System.ComponentModel.DataAnnotations => 0xd943a729 => 14
	i32 3657292374, ; 585: Microsoft.Extensions.Configuration.Abstractions.dll => 0xd9fdda56 => 187
	i32 3660523487, ; 586: System.Net.NetworkInformation => 0xda2f27df => 68
	i32 3672681054, ; 587: Mono.Android.dll => 0xdae8aa5e => 171
	i32 3682565725, ; 588: Xamarin.AndroidX.Browser => 0xdb7f7e5d => 231
	i32 3684561358, ; 589: Xamarin.AndroidX.Concurrent.Futures => 0xdb9df1ce => 235
	i32 3700591436, ; 590: Microsoft.IdentityModel.Abstractions.dll => 0xdc928b4c => 204
	i32 3700866549, ; 591: System.Net.WebProxy.dll => 0xdc96bdf5 => 78
	i32 3706696989, ; 592: Xamarin.AndroidX.Core.Core.Ktx.dll => 0xdcefb51d => 240
	i32 3716563718, ; 593: System.Runtime.Intrinsics => 0xdd864306 => 108
	i32 3718780102, ; 594: Xamarin.AndroidX.Annotation => 0xdda814c6 => 224
	i32 3722202641, ; 595: Microsoft.Extensions.Configuration.Json.dll => 0xdddc4e11 => 190
	i32 3724971120, ; 596: Xamarin.AndroidX.Navigation.Common.dll => 0xde068c70 => 266
	i32 3732100267, ; 597: System.Net.NameResolution => 0xde7354ab => 67
	i32 3732214720, ; 598: Microsoft.AspNetCore.Metadata.dll => 0xde7513c0 => 185
	i32 3737834244, ; 599: System.Net.Http.Json.dll => 0xdecad304 => 63
	i32 3748608112, ; 600: System.Diagnostics.DiagnosticSource => 0xdf6f3870 => 27
	i32 3751444290, ; 601: System.Xml.XPath => 0xdf9a7f42 => 160
	i32 3751619990, ; 602: da\Microsoft.Maui.Controls.resources.dll => 0xdf9d2d96 => 304
	i32 3758424670, ; 603: Microsoft.Extensions.Configuration.FileExtensions => 0xe005025e => 189
	i32 3786282454, ; 604: Xamarin.AndroidX.Collection => 0xe1ae15d6 => 233
	i32 3792276235, ; 605: System.Collections.NonGeneric => 0xe2098b0b => 10
	i32 3800979733, ; 606: Microsoft.Maui.Controls.Compatibility => 0xe28e5915 => 209
	i32 3802395368, ; 607: System.Collections.Specialized.dll => 0xe2a3f2e8 => 11
	i32 3817368567, ; 608: CommunityToolkit.Maui.dll => 0xe3886bf7 => 174
	i32 3819260425, ; 609: System.Net.WebProxy => 0xe3a54a09 => 78
	i32 3823082795, ; 610: System.Security.Cryptography.dll => 0xe3df9d2b => 126
	i32 3829621856, ; 611: System.Numerics.dll => 0xe4436460 => 83
	i32 3841636137, ; 612: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 0xe4fab729 => 192
	i32 3844307129, ; 613: System.Net.Mail.dll => 0xe52378b9 => 66
	i32 3849253459, ; 614: System.Runtime.InteropServices.dll => 0xe56ef253 => 107
	i32 3870376305, ; 615: System.Net.HttpListener.dll => 0xe6b14171 => 65
	i32 3873536506, ; 616: System.Security.Principal => 0xe6e179fa => 128
	i32 3875112723, ; 617: System.Security.Cryptography.Encoding.dll => 0xe6f98713 => 122
	i32 3885497537, ; 618: System.Net.WebHeaderCollection.dll => 0xe797fcc1 => 77
	i32 3885922214, ; 619: Xamarin.AndroidX.Transition.dll => 0xe79e77a6 => 281
	i32 3888767677, ; 620: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller => 0xe7c9e2bd => 271
	i32 3896106733, ; 621: System.Collections.Concurrent.dll => 0xe839deed => 8
	i32 3896760992, ; 622: Xamarin.AndroidX.Core.dll => 0xe843daa0 => 239
	i32 3901907137, ; 623: Microsoft.VisualBasic.Core.dll => 0xe89260c1 => 2
	i32 3920221145, ; 624: nl\Microsoft.Maui.Controls.resources.dll => 0xe9a9d3d9 => 320
	i32 3920810846, ; 625: System.IO.Compression.FileSystem.dll => 0xe9b2d35e => 44
	i32 3921031405, ; 626: Xamarin.AndroidX.VersionedParcelable.dll => 0xe9b630ed => 284
	i32 3928044579, ; 627: System.Xml.ReaderWriter => 0xea213423 => 156
	i32 3930554604, ; 628: System.Security.Principal.dll => 0xea4780ec => 128
	i32 3931092270, ; 629: Xamarin.AndroidX.Navigation.UI => 0xea4fb52e => 269
	i32 3945713374, ; 630: System.Data.DataSetExtensions.dll => 0xeb2ecede => 23
	i32 3953953790, ; 631: System.Text.Encoding.CodePages => 0xebac8bfe => 133
	i32 3955647286, ; 632: Xamarin.AndroidX.AppCompat.dll => 0xebc66336 => 227
	i32 3959773229, ; 633: Xamarin.AndroidX.Lifecycle.Process => 0xec05582d => 258
	i32 4003436829, ; 634: System.Diagnostics.Process.dll => 0xee9f991d => 29
	i32 4015948917, ; 635: Xamarin.AndroidX.Annotation.Jvm.dll => 0xef5e8475 => 226
	i32 4023392905, ; 636: System.IO.Pipelines => 0xefd01a89 => 217
	i32 4025784931, ; 637: System.Memory => 0xeff49a63 => 62
	i32 4046471985, ; 638: Microsoft.Maui.Controls.Xaml.dll => 0xf1304331 => 211
	i32 4054681211, ; 639: System.Reflection.Emit.ILGeneration => 0xf1ad867b => 90
	i32 4068434129, ; 640: System.Private.Xml.Linq.dll => 0xf27f60d1 => 87
	i32 4073602200, ; 641: System.Threading.dll => 0xf2ce3c98 => 148
	i32 4091086043, ; 642: el\Microsoft.Maui.Controls.resources.dll => 0xf3d904db => 306
	i32 4094352644, ; 643: Microsoft.Maui.Essentials.dll => 0xf40add04 => 213
	i32 4099507663, ; 644: System.Drawing.dll => 0xf45985cf => 36
	i32 4100113165, ; 645: System.Private.Uri => 0xf462c30d => 86
	i32 4101593132, ; 646: Xamarin.AndroidX.Emoji2 => 0xf479582c => 247
	i32 4103439459, ; 647: uk\Microsoft.Maui.Controls.resources.dll => 0xf4958463 => 330
	i32 4126470640, ; 648: Microsoft.Extensions.DependencyInjection => 0xf5f4f1f0 => 191
	i32 4127667938, ; 649: System.IO.FileSystem.Watcher => 0xf60736e2 => 50
	i32 4130442656, ; 650: System.AppContext => 0xf6318da0 => 6
	i32 4147896353, ; 651: System.Reflection.Emit.ILGeneration.dll => 0xf73be021 => 90
	i32 4150914736, ; 652: uk\Microsoft.Maui.Controls.resources => 0xf769eeb0 => 330
	i32 4151237749, ; 653: System.Core => 0xf76edc75 => 21
	i32 4159265925, ; 654: System.Xml.XmlSerializer => 0xf7e95c85 => 162
	i32 4161255271, ; 655: System.Reflection.TypeExtensions => 0xf807b767 => 96
	i32 4164802419, ; 656: System.IO.FileSystem.Watcher.dll => 0xf83dd773 => 50
	i32 4181436372, ; 657: System.Runtime.Serialization.Primitives => 0xf93ba7d4 => 113
	i32 4182413190, ; 658: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0xf94a8f86 => 263
	i32 4185676441, ; 659: System.Security => 0xf97c5a99 => 130
	i32 4196529839, ; 660: System.Net.WebClient.dll => 0xfa21f6af => 76
	i32 4213026141, ; 661: System.Diagnostics.DiagnosticSource.dll => 0xfb1dad5d => 27
	i32 4249188766, ; 662: nb\Microsoft.Maui.Controls.resources.dll => 0xfd45799e => 319
	i32 4256097574, ; 663: Xamarin.AndroidX.Core.Core.Ktx => 0xfdaee526 => 240
	i32 4258378803, ; 664: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx => 0xfdd1b433 => 262
	i32 4260525087, ; 665: System.Buffers => 0xfdf2741f => 7
	i32 4263231520, ; 666: System.IdentityModel.Tokens.Jwt.dll => 0xfe1bc020 => 216
	i32 4271975918, ; 667: Microsoft.Maui.Controls.dll => 0xfea12dee => 210
	i32 4274976490, ; 668: System.Runtime.Numerics => 0xfecef6ea => 110
	i32 4292120959, ; 669: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xffd4917f => 263
	i32 4294648842, ; 670: Microsoft.Extensions.FileProviders.Embedded => 0xfffb240a => 195
	i32 4294763496 ; 671: Xamarin.AndroidX.ExifInterface.dll => 0xfffce3e8 => 249
], align 4

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [672 x i32] [
	i32 68, ; 0
	i32 67, ; 1
	i32 108, ; 2
	i32 259, ; 3
	i32 293, ; 4
	i32 48, ; 5
	i32 301, ; 6
	i32 215, ; 7
	i32 80, ; 8
	i32 310, ; 9
	i32 145, ; 10
	i32 30, ; 11
	i32 334, ; 12
	i32 124, ; 13
	i32 214, ; 14
	i32 102, ; 15
	i32 318, ; 16
	i32 277, ; 17
	i32 107, ; 18
	i32 277, ; 19
	i32 139, ; 20
	i32 297, ; 21
	i32 333, ; 22
	i32 326, ; 23
	i32 77, ; 24
	i32 124, ; 25
	i32 13, ; 26
	i32 233, ; 27
	i32 132, ; 28
	i32 279, ; 29
	i32 151, ; 30
	i32 18, ; 31
	i32 231, ; 32
	i32 26, ; 33
	i32 253, ; 34
	i32 1, ; 35
	i32 59, ; 36
	i32 42, ; 37
	i32 179, ; 38
	i32 91, ; 39
	i32 178, ; 40
	i32 236, ; 41
	i32 147, ; 42
	i32 255, ; 43
	i32 252, ; 44
	i32 54, ; 45
	i32 69, ; 46
	i32 331, ; 47
	i32 222, ; 48
	i32 83, ; 49
	i32 309, ; 50
	i32 254, ; 51
	i32 131, ; 52
	i32 55, ; 53
	i32 149, ; 54
	i32 74, ; 55
	i32 145, ; 56
	i32 62, ; 57
	i32 146, ; 58
	i32 335, ; 59
	i32 165, ; 60
	i32 329, ; 61
	i32 237, ; 62
	i32 12, ; 63
	i32 250, ; 64
	i32 125, ; 65
	i32 152, ; 66
	i32 113, ; 67
	i32 166, ; 68
	i32 164, ; 69
	i32 252, ; 70
	i32 204, ; 71
	i32 265, ; 72
	i32 307, ; 73
	i32 84, ; 74
	i32 202, ; 75
	i32 150, ; 76
	i32 297, ; 77
	i32 60, ; 78
	i32 328, ; 79
	i32 198, ; 80
	i32 51, ; 81
	i32 103, ; 82
	i32 114, ; 83
	i32 40, ; 84
	i32 290, ; 85
	i32 288, ; 86
	i32 195, ; 87
	i32 120, ; 88
	i32 174, ; 89
	i32 52, ; 90
	i32 44, ; 91
	i32 119, ; 92
	i32 242, ; 93
	i32 320, ; 94
	i32 248, ; 95
	i32 81, ; 96
	i32 136, ; 97
	i32 284, ; 98
	i32 229, ; 99
	i32 8, ; 100
	i32 73, ; 101
	i32 155, ; 102
	i32 299, ; 103
	i32 154, ; 104
	i32 92, ; 105
	i32 294, ; 106
	i32 45, ; 107
	i32 298, ; 108
	i32 109, ; 109
	i32 129, ; 110
	i32 25, ; 111
	i32 219, ; 112
	i32 72, ; 113
	i32 55, ; 114
	i32 46, ; 115
	i32 326, ; 116
	i32 201, ; 117
	i32 243, ; 118
	i32 184, ; 119
	i32 22, ; 120
	i32 257, ; 121
	i32 86, ; 122
	i32 43, ; 123
	i32 160, ; 124
	i32 71, ; 125
	i32 270, ; 126
	i32 311, ; 127
	i32 3, ; 128
	i32 42, ; 129
	i32 63, ; 130
	i32 325, ; 131
	i32 16, ; 132
	i32 53, ; 133
	i32 322, ; 134
	i32 293, ; 135
	i32 105, ; 136
	i32 215, ; 137
	i32 298, ; 138
	i32 315, ; 139
	i32 291, ; 140
	i32 254, ; 141
	i32 34, ; 142
	i32 158, ; 143
	i32 85, ; 144
	i32 32, ; 145
	i32 324, ; 146
	i32 12, ; 147
	i32 51, ; 148
	i32 197, ; 149
	i32 56, ; 150
	i32 274, ; 151
	i32 36, ; 152
	i32 192, ; 153
	i32 292, ; 154
	i32 227, ; 155
	i32 35, ; 156
	i32 305, ; 157
	i32 58, ; 158
	i32 261, ; 159
	i32 176, ; 160
	i32 17, ; 161
	i32 295, ; 162
	i32 164, ; 163
	i32 189, ; 164
	i32 327, ; 165
	i32 321, ; 166
	i32 317, ; 167
	i32 260, ; 168
	i32 200, ; 169
	i32 287, ; 170
	i32 323, ; 171
	i32 153, ; 172
	i32 193, ; 173
	i32 283, ; 174
	i32 268, ; 175
	i32 229, ; 176
	i32 29, ; 177
	i32 52, ; 178
	i32 0, ; 179
	i32 288, ; 180
	i32 5, ; 181
	i32 303, ; 182
	i32 278, ; 183
	i32 282, ; 184
	i32 234, ; 185
	i32 299, ; 186
	i32 226, ; 187
	i32 245, ; 188
	i32 312, ; 189
	i32 85, ; 190
	i32 287, ; 191
	i32 61, ; 192
	i32 112, ; 193
	i32 332, ; 194
	i32 57, ; 195
	i32 333, ; 196
	i32 274, ; 197
	i32 99, ; 198
	i32 19, ; 199
	i32 238, ; 200
	i32 111, ; 201
	i32 101, ; 202
	i32 102, ; 203
	i32 301, ; 204
	i32 104, ; 205
	i32 291, ; 206
	i32 71, ; 207
	i32 38, ; 208
	i32 32, ; 209
	i32 194, ; 210
	i32 103, ; 211
	i32 73, ; 212
	i32 216, ; 213
	i32 307, ; 214
	i32 9, ; 215
	i32 123, ; 216
	i32 46, ; 217
	i32 228, ; 218
	i32 202, ; 219
	i32 9, ; 220
	i32 43, ; 221
	i32 4, ; 222
	i32 275, ; 223
	i32 205, ; 224
	i32 197, ; 225
	i32 331, ; 226
	i32 31, ; 227
	i32 138, ; 228
	i32 92, ; 229
	i32 184, ; 230
	i32 93, ; 231
	i32 49, ; 232
	i32 141, ; 233
	i32 112, ; 234
	i32 140, ; 235
	i32 244, ; 236
	i32 115, ; 237
	i32 292, ; 238
	i32 157, ; 239
	i32 76, ; 240
	i32 79, ; 241
	i32 264, ; 242
	i32 37, ; 243
	i32 286, ; 244
	i32 190, ; 245
	i32 175, ; 246
	i32 248, ; 247
	i32 241, ; 248
	i32 64, ; 249
	i32 138, ; 250
	i32 15, ; 251
	i32 183, ; 252
	i32 116, ; 253
	i32 280, ; 254
	i32 289, ; 255
	i32 236, ; 256
	i32 48, ; 257
	i32 70, ; 258
	i32 80, ; 259
	i32 173, ; 260
	i32 126, ; 261
	i32 94, ; 262
	i32 121, ; 263
	i32 296, ; 264
	i32 26, ; 265
	i32 257, ; 266
	i32 97, ; 267
	i32 28, ; 268
	i32 232, ; 269
	i32 302, ; 270
	i32 149, ; 271
	i32 217, ; 272
	i32 169, ; 273
	i32 4, ; 274
	i32 98, ; 275
	i32 181, ; 276
	i32 33, ; 277
	i32 93, ; 278
	i32 279, ; 279
	i32 198, ; 280
	i32 21, ; 281
	i32 41, ; 282
	i32 170, ; 283
	i32 318, ; 284
	i32 250, ; 285
	i32 310, ; 286
	i32 203, ; 287
	i32 264, ; 288
	i32 295, ; 289
	i32 289, ; 290
	i32 269, ; 291
	i32 2, ; 292
	i32 134, ; 293
	i32 111, ; 294
	i32 182, ; 295
	i32 199, ; 296
	i32 219, ; 297
	i32 327, ; 298
	i32 58, ; 299
	i32 95, ; 300
	i32 309, ; 301
	i32 39, ; 302
	i32 230, ; 303
	i32 25, ; 304
	i32 94, ; 305
	i32 303, ; 306
	i32 89, ; 307
	i32 99, ; 308
	i32 10, ; 309
	i32 87, ; 310
	i32 314, ; 311
	i32 100, ; 312
	i32 276, ; 313
	i32 186, ; 314
	i32 296, ; 315
	i32 221, ; 316
	i32 207, ; 317
	i32 306, ; 318
	i32 7, ; 319
	i32 261, ; 320
	i32 218, ; 321
	i32 88, ; 322
	i32 188, ; 323
	i32 256, ; 324
	i32 154, ; 325
	i32 305, ; 326
	i32 33, ; 327
	i32 196, ; 328
	i32 116, ; 329
	i32 82, ; 330
	i32 20, ; 331
	i32 11, ; 332
	i32 162, ; 333
	i32 3, ; 334
	i32 212, ; 335
	i32 313, ; 336
	i32 201, ; 337
	i32 199, ; 338
	i32 179, ; 339
	i32 84, ; 340
	i32 300, ; 341
	i32 64, ; 342
	i32 315, ; 343
	i32 283, ; 344
	i32 143, ; 345
	i32 265, ; 346
	i32 157, ; 347
	i32 41, ; 348
	i32 117, ; 349
	i32 187, ; 350
	i32 220, ; 351
	i32 272, ; 352
	i32 131, ; 353
	i32 75, ; 354
	i32 66, ; 355
	i32 319, ; 356
	i32 172, ; 357
	i32 173, ; 358
	i32 224, ; 359
	i32 143, ; 360
	i32 106, ; 361
	i32 151, ; 362
	i32 70, ; 363
	i32 313, ; 364
	i32 156, ; 365
	i32 206, ; 366
	i32 186, ; 367
	i32 121, ; 368
	i32 127, ; 369
	i32 314, ; 370
	i32 152, ; 371
	i32 247, ; 372
	i32 178, ; 373
	i32 141, ; 374
	i32 234, ; 375
	i32 311, ; 376
	i32 20, ; 377
	i32 14, ; 378
	i32 208, ; 379
	i32 135, ; 380
	i32 75, ; 381
	i32 59, ; 382
	i32 237, ; 383
	i32 167, ; 384
	i32 168, ; 385
	i32 210, ; 386
	i32 15, ; 387
	i32 74, ; 388
	i32 6, ; 389
	i32 23, ; 390
	i32 317, ; 391
	i32 259, ; 392
	i32 218, ; 393
	i32 177, ; 394
	i32 91, ; 395
	i32 312, ; 396
	i32 1, ; 397
	i32 136, ; 398
	i32 316, ; 399
	i32 260, ; 400
	i32 282, ; 401
	i32 134, ; 402
	i32 69, ; 403
	i32 180, ; 404
	i32 146, ; 405
	i32 193, ; 406
	i32 321, ; 407
	i32 300, ; 408
	i32 251, ; 409
	i32 200, ; 410
	i32 88, ; 411
	i32 96, ; 412
	i32 241, ; 413
	i32 246, ; 414
	i32 316, ; 415
	i32 31, ; 416
	i32 45, ; 417
	i32 255, ; 418
	i32 206, ; 419
	i32 182, ; 420
	i32 220, ; 421
	i32 109, ; 422
	i32 158, ; 423
	i32 35, ; 424
	i32 22, ; 425
	i32 114, ; 426
	i32 183, ; 427
	i32 57, ; 428
	i32 280, ; 429
	i32 144, ; 430
	i32 118, ; 431
	i32 120, ; 432
	i32 110, ; 433
	i32 222, ; 434
	i32 139, ; 435
	i32 177, ; 436
	i32 228, ; 437
	i32 203, ; 438
	i32 302, ; 439
	i32 54, ; 440
	i32 105, ; 441
	i32 322, ; 442
	i32 211, ; 443
	i32 212, ; 444
	i32 133, ; 445
	i32 294, ; 446
	i32 285, ; 447
	i32 273, ; 448
	i32 328, ; 449
	i32 251, ; 450
	i32 214, ; 451
	i32 159, ; 452
	i32 238, ; 453
	i32 163, ; 454
	i32 132, ; 455
	i32 273, ; 456
	i32 161, ; 457
	i32 185, ; 458
	i32 262, ; 459
	i32 140, ; 460
	i32 285, ; 461
	i32 281, ; 462
	i32 169, ; 463
	i32 213, ; 464
	i32 175, ; 465
	i32 223, ; 466
	i32 290, ; 467
	i32 40, ; 468
	i32 249, ; 469
	i32 81, ; 470
	i32 181, ; 471
	i32 56, ; 472
	i32 37, ; 473
	i32 97, ; 474
	i32 166, ; 475
	i32 172, ; 476
	i32 196, ; 477
	i32 286, ; 478
	i32 82, ; 479
	i32 225, ; 480
	i32 98, ; 481
	i32 30, ; 482
	i32 159, ; 483
	i32 18, ; 484
	i32 127, ; 485
	i32 119, ; 486
	i32 245, ; 487
	i32 276, ; 488
	i32 258, ; 489
	i32 278, ; 490
	i32 165, ; 491
	i32 253, ; 492
	i32 335, ; 493
	i32 308, ; 494
	i32 275, ; 495
	i32 266, ; 496
	i32 170, ; 497
	i32 16, ; 498
	i32 144, ; 499
	i32 207, ; 500
	i32 125, ; 501
	i32 118, ; 502
	i32 0, ; 503
	i32 38, ; 504
	i32 115, ; 505
	i32 47, ; 506
	i32 142, ; 507
	i32 117, ; 508
	i32 34, ; 509
	i32 176, ; 510
	i32 95, ; 511
	i32 53, ; 512
	i32 267, ; 513
	i32 129, ; 514
	i32 153, ; 515
	i32 24, ; 516
	i32 161, ; 517
	i32 244, ; 518
	i32 148, ; 519
	i32 104, ; 520
	i32 89, ; 521
	i32 232, ; 522
	i32 60, ; 523
	i32 142, ; 524
	i32 100, ; 525
	i32 5, ; 526
	i32 13, ; 527
	i32 122, ; 528
	i32 135, ; 529
	i32 28, ; 530
	i32 308, ; 531
	i32 205, ; 532
	i32 72, ; 533
	i32 242, ; 534
	i32 24, ; 535
	i32 230, ; 536
	i32 271, ; 537
	i32 268, ; 538
	i32 325, ; 539
	i32 137, ; 540
	i32 223, ; 541
	i32 239, ; 542
	i32 168, ; 543
	i32 272, ; 544
	i32 304, ; 545
	i32 101, ; 546
	i32 123, ; 547
	i32 243, ; 548
	i32 194, ; 549
	i32 188, ; 550
	i32 191, ; 551
	i32 163, ; 552
	i32 167, ; 553
	i32 246, ; 554
	i32 39, ; 555
	i32 209, ; 556
	i32 323, ; 557
	i32 180, ; 558
	i32 17, ; 559
	i32 171, ; 560
	i32 324, ; 561
	i32 137, ; 562
	i32 150, ; 563
	i32 235, ; 564
	i32 208, ; 565
	i32 155, ; 566
	i32 130, ; 567
	i32 19, ; 568
	i32 65, ; 569
	i32 147, ; 570
	i32 47, ; 571
	i32 332, ; 572
	i32 334, ; 573
	i32 221, ; 574
	i32 79, ; 575
	i32 61, ; 576
	i32 106, ; 577
	i32 270, ; 578
	i32 225, ; 579
	i32 49, ; 580
	i32 256, ; 581
	i32 329, ; 582
	i32 267, ; 583
	i32 14, ; 584
	i32 187, ; 585
	i32 68, ; 586
	i32 171, ; 587
	i32 231, ; 588
	i32 235, ; 589
	i32 204, ; 590
	i32 78, ; 591
	i32 240, ; 592
	i32 108, ; 593
	i32 224, ; 594
	i32 190, ; 595
	i32 266, ; 596
	i32 67, ; 597
	i32 185, ; 598
	i32 63, ; 599
	i32 27, ; 600
	i32 160, ; 601
	i32 304, ; 602
	i32 189, ; 603
	i32 233, ; 604
	i32 10, ; 605
	i32 209, ; 606
	i32 11, ; 607
	i32 174, ; 608
	i32 78, ; 609
	i32 126, ; 610
	i32 83, ; 611
	i32 192, ; 612
	i32 66, ; 613
	i32 107, ; 614
	i32 65, ; 615
	i32 128, ; 616
	i32 122, ; 617
	i32 77, ; 618
	i32 281, ; 619
	i32 271, ; 620
	i32 8, ; 621
	i32 239, ; 622
	i32 2, ; 623
	i32 320, ; 624
	i32 44, ; 625
	i32 284, ; 626
	i32 156, ; 627
	i32 128, ; 628
	i32 269, ; 629
	i32 23, ; 630
	i32 133, ; 631
	i32 227, ; 632
	i32 258, ; 633
	i32 29, ; 634
	i32 226, ; 635
	i32 217, ; 636
	i32 62, ; 637
	i32 211, ; 638
	i32 90, ; 639
	i32 87, ; 640
	i32 148, ; 641
	i32 306, ; 642
	i32 213, ; 643
	i32 36, ; 644
	i32 86, ; 645
	i32 247, ; 646
	i32 330, ; 647
	i32 191, ; 648
	i32 50, ; 649
	i32 6, ; 650
	i32 90, ; 651
	i32 330, ; 652
	i32 21, ; 653
	i32 162, ; 654
	i32 96, ; 655
	i32 50, ; 656
	i32 113, ; 657
	i32 263, ; 658
	i32 130, ; 659
	i32 76, ; 660
	i32 27, ; 661
	i32 319, ; 662
	i32 240, ; 663
	i32 262, ; 664
	i32 7, ; 665
	i32 216, ; 666
	i32 210, ; 667
	i32 110, ; 668
	i32 263, ; 669
	i32 195, ; 670
	i32 249 ; 671
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
