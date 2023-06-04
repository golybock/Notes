import React from "react";
import "./Auth.css"
import sakura from '/src/sakura.jpg'
import AuthApi from "../../api/user/AuthApi";
import {Modal} from "react-bootstrap";
import {GoogleLogin} from 'react-google-login'


class SignIn extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            email: "aboba12@aboba.com",
            password: "Password1!",
            error: ""
        }
    }

    client_id = "989073554490-lhcf948sulr8o8n5u85ivnbbluh3ve51.apps.googleusercontent.com"

    auth = async () => {

        let r = await AuthApi.login(this.state.email, this.state.password)

        if (r === true) {
            this.props.onClose()
        } else {
            this.setState({error: "Неверный логин или пароль"});
        }
    }

    onSuccess = (res) => {
        console.log("es", res.profileObj)
    }

    onFailure = (res) => {
        console.log("no", res)
    }

    render() {
        return (<div>

            <Modal.Dialog>

                <Modal.Body>

                    <div className="container">
                        <div className="body d-md-flex align-items-center justify-content-between">
                            <div className="box-1 mt-md-0 mt-5">
                                <img
                                    src={sakura}
                                    alt={sakura}/>
                            </div>
                            <div className=" box-2 d-flex flex-column h-100">
                                <div className="mt-5">
                                    <p className="mb-1 h-1">Sign in.</p>
                                    <div className="d-flex flex-column ">
                                        <div className="mb-3">
                                            <label>Email address</label>
                                            <input
                                                type="email"
                                                className="form-control"
                                                placeholder="Enter email"
                                                value={this.state.email}
                                                onChange={(e) => {
                                                    this.setState({
                                                        email: e.target.value
                                                    })
                                                }}
                                            />
                                        </div>
                                        <div className="mb-3">
                                            <label>Password</label>
                                            <input
                                                type="password"
                                                className="form-control"
                                                placeholder="Enter password"
                                                value={this.state.password}
                                                onChange={(e) => {
                                                    this.setState({
                                                        password: e.target.value
                                                    })
                                                }}
                                            />
                                        </div>
                                        <div className="d-grid">
                                            <button onClick={async () => this.auth()}
                                                    className="btn btn-primary-submit">
                                                Sign in
                                            </button>
                                            <div id="signInButton">
                                                <GoogleLogin clientId={this.client_id}
                                                             buttonText="Login"
                                                             onSuccess={this.onSuccess}
                                                             onFailure={this.onFailure}
                                                             cookiePolicy={'single_host_origin'}
                                                             isSignedIn={true}/>

                                            </div>
                                        </div>
                                        <div className="mt-3">
                                            <p className="mb-0 text-muted">Dont have an account?</p>
                                            <button className="btn btn-primary" onClick={() => {
                                            }}>Registration
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            {/*<span className="fas fa-times">*/}
                            {/*    <img src={close} alt={close}/>*/}
                            {/*</span>*/}
                        </div>
                    </div>

                </Modal.Body>
                {/*Кнопка добавления*/}
                {/*<Button variant="success"*/}
                {/*        style={this.props.isEdit ? {display: 'none'} : {display: 'flex'}}*/}
                {/*        onClick={async () => {*/}
                {/*            try {*/}
                {/*                // создаем новый объект класса из введенных данных*/}
                {/*                console.log(this.state.product)*/}
                {/*                // отправляем запрос на добавление данных и получаем ответ в виде статуса*/}
                {/*                let resCode = await Api.addProduct(this.state.product);*/}
                {/*                // оповещаем об ответе сервера*/}
                {/*                if (resCode !== 400) {*/}
                {/*                    alert("Добавлено!");*/}
                {/*                    this.props.onClose();*/}
                {/*                } else if (resCode === 400) {*/}
                {/*                    alert("Неверные данные!")*/}
                {/*                }*/}
                {/*            } catch (Ex) {*/}
                {/*                alert(Ex);*/}
                {/*            }*/}

                {/*        }}>*/}
                {/*    Добавить*/}
                {/*</Button>*/}

                {/*/!*Кнопка изменения*!/*/}
                {/*<Button variant="success"*/}
                {/*        style={this.props.isEdit ? {display: 'flex'} : {display: 'none'}}*/}
                {/*        onClick={async () => {*/}

                {/*        }}>*/}
                {/*    Изменить*/}
                {/*</Button>*/}


            </Modal.Dialog>

        </div>)

    }
}

export default SignIn;
