export default class ApiBase {

    static baseAddress = "https://localhost:7058/api"

    // go to main page to show login window
    navigateToAuth(){
        window.location.replace("http://localhost:3000")
    }

}