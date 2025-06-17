using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using SFB;

public class GetFiles : MonoBehaviour
{

    public static GetFiles ins;

	public Image PP;

	Sprite img;

    Texture2D screenshot;

    public byte[] imageByte;


    public void Awake()
    {
         ins = this;
    }


    public void OnClick(int maxSize)
    {
		//PP.sprite = PickImage(maxSize);

       // StartCoroutine(TakeImageAndCrop());  

    }

    private Sprite PickImage(int maxSize)
	{
		NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
		{
			Debug.Log("Image path: " + path);
			if (path != null)
			{
				// Create Texture from selected image
				Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);
				if (texture == null)
				{
					Debug.Log("Couldn't load texture from " + path);
					return;
				}
				img = Sprite.Create(texture, new Rect(0,0,texture.width, texture.height), new Vector2(.5f, .5f));
			}
		});

		Debug.Log("Permission result: " + permission);
		return img;
	}

    public void OnClickOpenFilePanel()
    {
        var extensions = new[] { new ExtensionFilter("Image Files", "png", "jpg", "jpeg") };
        var paths = StandaloneFileBrowser.OpenFilePanel("Select an Image", "", extensions, false);

        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
        {
            // StartCoroutine(LoadAndCropImage(paths[0]));
        }
        else
        {
            Debug.LogError("No file selected.");
        }
    }


    /* private IEnumerator TakeImageAndCrop()
     {
         yield return new WaitForEndOfFrame();

         bool ovalSelection = false;
         bool autoZoom = false;

         float minAspectRatio, maxAspectRatio;

         minAspectRatio = 1f;
         maxAspectRatio = 1f;

         NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
         {
             Debug.Log("Image path: " + path);
             if (path != null)
             {
                 // Create Texture from selected image
                 screenshot = NativeGallery.LoadImageAtPath(path, -1);
                 if (screenshot == null)
                 {
                     Debug.Log("Couldn't load texture from " + path);
                     return;
                 }
             }
         });

         //Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
         //screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
         //screenshot.Apply();

         ImageCropper.Instance.Show(screenshot, (bool result, Texture originalImage, Texture2D croppedImage) =>
         {
             // Destroy previously cropped texture (if any) to free memory
             //Destroy(croppedImageHolder.texture, 5f);

             // If screenshot was cropped successfully
             if (result)
             {
                 img = Sprite.Create(croppedImage, new Rect(0, 0, croppedImage.width, croppedImage.height), new Vector2(.5f, .5f));
                 PP.sprite = img;
                 imageByte = croppedImage.GetRawTextureData();
             } 
             // Destroy the screenshot as we no longer need it in this case
             Destroy(screenshot);  

         },
         settings: new ImageCropper.Settings()
         {
             ovalSelection = ovalSelection,
             autoZoomEnabled = autoZoom,
             imageBackground = Color.clear, // transparent background
             selectionMinAspectRatio = minAspectRatio,
             selectionMaxAspectRatio = maxAspectRatio

         },
         croppedImageResizePolicy: (ref int width, ref int height) =>
         {
             // uncomment lines below to save cropped image at half resolution
             //width /= 2;
             //height /= 2;
         });
     }*/


    public IEnumerator LoadAndCropImage(string filePath)
    {
        byte[] fileData = System.IO.File.ReadAllBytes(filePath);
        screenshot = new Texture2D(2, 2);
        screenshot.LoadImage(fileData);
        yield return new WaitForEndOfFrame();

        ImageCropper.Instance.Show(screenshot, (bool result, Texture originalImage, Texture2D croppedImage) =>
        {
            if (result)
            {
                //img = Sprite.Create(croppedImage, new Rect(0, 0, croppedImage.width, croppedImage.height), new Vector2(.5f, .5f));

                img = Sprite.Create(croppedImage, new Rect(0, 0, croppedImage.width, croppedImage.height), new Vector2(.5f, .5f));
                PP.sprite = img;
                imageByte = croppedImage.GetRawTextureData();

                /* if(updateProfile != null)
                 {
                     if (updateProfile.imageTest == 1)
                     {
                         ImageSize = new Vector2(248, 424);
                         PPm.sprite = img;
                         PPm.rectTransform.sizeDelta = ImageSize;
                         Debug.Log("Profile picture updated for male.");
                     }
                     else if (updateProfile.imageTest == 2)
                     {
                         ImageSize = new Vector2(248, 262);
                         PPf.sprite = img;
                         PPf.rectTransform.sizeDelta = ImageSize;
                         Debug.Log("Profile picture updated for female.");
                     }  

                 }*/

                Debug.Log("hello hello hello hello ");

                /* if (updateProfileMale.imageTest == 1 && updateProfileMale.isImageTypeMale)
                 {
                     Debug.Log("hello hello hello hello 1111");
                     ImageSize = new Vector2(248, 424);
                     PPm.sprite = img;
                     PPm.rectTransform.sizeDelta = ImageSize;
                     Debug.Log("Profile picture updated for male.");
                     updateProfileMale.isImageTypeMale = false;

                 }
                 else if (updateProfileFeMale.imageTest == 2 && updateProfileFeMale.isImageTypeFeMale)
                 {
                     Debug.Log("hello hello hello hello 2222222");
                     ImageSize = new Vector2(248, 262);
                     PPf.sprite = img;
                     PPf.rectTransform.sizeDelta = ImageSize;
                     Debug.Log("Profile picture updated for female.");
                     updateProfileFeMale.isImageTypeFeMale = false;
                 }*/

                /* else if(createProfile != null)
                 {
                     if (createProfile.imageTest == 1)
                     {
                         ImageSize = new Vector2(248, 424);
                         PPm.sprite = img;
                         PPm.rectTransform.sizeDelta = ImageSize;
                         Debug.Log("Profile picture updated for male.");
                     }
                     else if (createProfile.imageTest == 2)
                     {
                         ImageSize = new Vector2(248, 262);
                         PPf.sprite = img;
                         PPf.rectTransform.sizeDelta = ImageSize;
                         Debug.Log("Profile picture updated for female.");
                     }
                 }*/

                //PPm.sprite = img;
                //imageByte = croppedImage.GetRawTextureData();
            }
            // Destroy the screenshot as we no longer need it in this case
            Destroy(screenshot);

        },
            settings: new ImageCropper.Settings()
            {
                ovalSelection = false, // Enable oval selection
                autoZoomEnabled = false, // Enable auto zoom for better UX
                imageBackground = Color.clear, // Set transparent background
                selectionMinAspectRatio = 1f, // Minimum aspect ratio for cropping
                selectionMaxAspectRatio = 1f // Maximum aspect ratio for cropping
            },
        croppedImageResizePolicy: (ref int width, ref int height) =>
        {
            // Resize the cropped image (optional)
            //  width /= 2; // Resize width to half
            //  height /= 2; // Resize height to half
        });
    }

    public void ResetImage()
    {

        PP.sprite = null;

        /*if (PPm != null)
        {
            PPm.sprite = null;
            Debug.Log("Male profile picture reset.");
        }

        if (PPf != null)
        {
            PPf.sprite = null;
            Debug.Log("Female profile picture reset.");
        }*/
    }

}
