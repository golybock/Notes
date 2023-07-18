import axios from "axios";

export default class ApiBase {

    static baseAddress = "https://localhost:7058/api"

    constructor() {
        axios.defaults.withCredentials = true
    }
    
    // go to main page to show login window
    navigateToMain() {
        window.location.replace("http://localhost:3000")
    }
}