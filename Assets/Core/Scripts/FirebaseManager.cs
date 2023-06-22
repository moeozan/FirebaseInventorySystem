using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using Firebase.Firestore;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager instance;
    IDictionary<string, UserData> keyValuePairs;

    public TMP_Text informationText;
    //Firebase variables
    [Header("Firebase")]
    private DependencyStatus dependencyStatus;
    private FirebaseAuth auth;
    private FirebaseUser User;

    //Login variables
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;

    //Register variables
    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;

    [Header("GamePanel")]
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject loadingPanel;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase()
    {
        //Set the authentication instance object
        Debug.Log("Setting up Firebase Auth");
        auth = FirebaseAuth.DefaultInstance;
    }

    //Function for the login button
    public void LoginButton()
    {
        //Call the login coroutine passing the email and password
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }
    //Function for the register button
    public void RegisterButton()
    {
        //Call the register coroutine passing the email, password, and username
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }

    private IEnumerator Login(string _email, string _password)
    {
        informationText.text = "Logging";
        loadingPanel.SetActive(true);
        //Call the Firebase auth signin function passing the email and password
        var LoginTask =  auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);
        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            informationText.text = message;
        }
        else
        {
            //User is now logged in
            //Now get the result
            User = LoginTask.Result.User;
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.Email, User.DisplayName);
            informationText.text = "Logged In";
            loadingPanel.SetActive(false);
            gamePanel.SetActive(true);
            informationText.text = "";
        }
    }



    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            //If the username field is blank show a warning
            informationText.text = "Missing Username";
        }
        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            //If the password does not match show a warning
            informationText.text = "Password Does Not Match!";
        }
        else
        {
            //Call the Firebase auth signin function passing the email and password

            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);

            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                informationText.text = message;
            }
            else
            {
                //User has now been created
                //Now get the result
                if (RegisterTask.Result != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile { DisplayName = _username };
                    //Call the Firebase auth update user profile function passing the profile with the username

                    User = RegisterTask.Result.User;

                    var ProfileTask = User.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        informationText.text = "Username Set Failed!";
                    }
                    else
                    {
                        //Username is now set
                        //Now return to login screen

                        UserData userdata = new(emailRegisterField.text, passwordRegisterField.text);
                        StartCoroutine(SaveUserToFireStore(userdata, User.DisplayName));
                        informationText.text = "Register Success";
                        Debug.Log(userdata.UserInventory.Slots.Length);
                    }
                }
            }
        }
    }
    public IEnumerator SaveUserToFireStore(UserData data, string documentId)
    {
        FirebaseFirestore _firestore = FirebaseFirestore.DefaultInstance;
        CollectionReference collectionRef = _firestore.Collection("User");
        DocumentReference documentRef = collectionRef.Document(documentId);
        var task = documentRef.SetAsync(data);
        yield return new WaitUntil(predicate: () => task.IsCompleted);
        if (task.IsCompleted)
        {
            Debug.Log("Custom object saved with ID: " + documentRef.Id);
        }

        else if (task.IsFaulted)
        {
            Debug.Log("Error : " + task.Exception.InnerException);
        }
    }

    public async Task<Inventory> GetInventoryInformation()
    {
        FirebaseFirestore _firestore = FirebaseFirestore.DefaultInstance;
        CollectionReference collectionRef = _firestore.Collection("User");
        DocumentReference documentRef = collectionRef.Document(auth.CurrentUser.DisplayName);
        DocumentSnapshot data = await documentRef.GetSnapshotAsync();
        UserData userData = data.ConvertTo<UserData>();

        return userData.UserInventory;
    }

    public IEnumerator UpdateInventoryInformation(Inventory inventory)
    {
        FirebaseFirestore _firestore = FirebaseFirestore.DefaultInstance;
        CollectionReference collectionRef = _firestore.Collection("User");
        DocumentReference documentRef = collectionRef.Document(auth.CurrentUser.DisplayName);
        var data = new UserData
        {
            UserInventory = inventory
        };

        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "UserInventory", inventory }
        };

        var task = documentRef.UpdateAsync(updates);

        yield return new WaitUntil(predicate: () => task.IsCompleted);
        if (task.IsCompleted)
        {
            Debug.Log("Inventory Updated for : " + documentRef.Id);
        }
        else if (task.IsFaulted)
        {
            Debug.Log("Error : " + task.Exception.InnerException);
        }
    }
}