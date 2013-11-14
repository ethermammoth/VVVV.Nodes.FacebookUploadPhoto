#region usings
using System;
using System.Net;
using System.Text;
using System.IO;
using System.Web;
using VVVV.PluginInterfaces.V2;
#endregion usings

namespace VVVV.Nodes
{
	#region PluginInfo
	[PluginInfo(Name = "UploadPhoto", Category = "Facebook", Help = "Upload photo to album", Author = "tgd")]
	#endregion PluginInfo
	public class FacebookUploadPhotoNode : IPluginEvaluate
	{
		#region fields & pins
		[Input("Upload", IsBang=true, IsSingle=true)]
        ISpread<bool> FBang;
		
		[Input("AlbumID", IsSingle=true)]
		ISpread<string> FAlbumID;
		
		[Input("Photo Name", IsSingle=true)]
		ISpread<string> FPhotoName;
		
		[Input("Filepath", IsSingle=true, StringType = StringType.Filename)]
		ISpread<string> FFilepath;
		
		[Input("Access Token", IsSingle=true)]
		ISpread<string> FAccessToken;
		
		[Output("PhotoID")]
		ISpread<string> FPhotoID;
		
		[Output("Status")]
		ISpread<string> FStatus;
		#endregion fields & pins
			
	
		public void Evaluate(int SpreadMax)
		{
			if (FBang[0]) 
			
		try {
			 if (File.Exists(FFilepath[0]))
				  {
			 FStatus[0] = "";
			 FPhotoID[0] = "";
			  
			 string theURI = ("https://graph.facebook.com/" + FAlbumID[0] + "/photos"
				+ "?message=" + HttpUtility.UrlEncode(FPhotoName[0], Encoding.UTF8) + "&access_token=" + FAccessToken[0]);
 
  			 WebClient myWebClient = new WebClient();
 			 myWebClient.UploadProgressChanged += new UploadProgressChangedEventHandler(Progress);
 			 myWebClient.UploadFileCompleted += new UploadFileCompletedEventHandler(Complete);
  			 myWebClient.UploadFileAsync(new Uri(theURI), FFilepath[0]);
 			      }
			 else {	FStatus[0] = "File not found";}
			
		 	 } catch(Exception error) { FStatus[0] = error.Message;}					
				
		}
	
		
		public void Progress(object sender, UploadProgressChangedEventArgs e)
		{
  		FStatus[0] = ((e.ProgressPercentage *2).ToString() + "%");
 		}
		
		
		public void Complete(object sender, UploadFileCompletedEventArgs e)
		{            
  			try {
  			string response = Encoding.UTF8.GetString(e.Result);
  
			if (response.Contains("id")) // response is id or error?
	
			{
			 FStatus[0] = "Upload ok!";	
	 		 response = response.Remove(0,7);
	 	     response = response.Remove((response.Length-2),2);	
	 	     FPhotoID[0] = response;
			}		
				
		    else {FStatus[0] = response;} 
  				
  				} catch(Exception error) { FStatus[0] = error.Message;}	
  			
		}
		
	}
}
