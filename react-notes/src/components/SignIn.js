import React from "react";
import "./Login.css"
import {CloseButton, Modal, Button} from "react-bootstrap";
import AuthApi from "../api/user/AuthApi";


class SignIn extends React.Component {

  constructor(props) {
    super(props);

    this.state = {
      email : "aboba12@aboba.com",
      password : "Password1!"
    }
  }
  render() {
    return <div className="container">

      <Modal.Dialog className="container_modal_content">
        <Modal.Header>
          <Modal.Title>Добавление/Изменение продукта</Modal.Title>
          <CloseButton onClick={() => this.props.onClose()}/>
        </Modal.Header>

        <Modal.Body>
          <div className="container">
            <div className="row">
              <div className="col-md-5 mx-auto">
                <div className="card card-body">

                  <form onSubmit={async () => {
                    try {
                      let res = await AuthApi.login(this.state.email, this.state.password);

                      if (res === false)
                        alert("Неверный логин или пароль")

                      if (res === true)
                        alert("Вход выполнен")
                    } catch (e) {
                      console.log(e)
                      alert(e)
                    }
                  }}>
                    <div className="form-group required">
                      <label>Username / Email</label>
                      <input type="email"
                             placeholder="Email"
                             value={this.state.email}
                             onChange={(e) => {
                               this.setState({
                                 email: e.target.value
                               })
                             }}></input>
                    </div>
                    <div>
                      <label>Password</label>
                      <input type="password"
                             placeholder="Password"
                             value={this.state.password}
                             onChange={(e) => {
                               this.setState({
                                 password : e.target.value
                               })
                             }}/>
                    </div>
                    <div>
                      <Button className="btn btn-primary btn-block"
                              onClick={() => this.props.onClose()}>Отмена</Button>
                      <Button className="btn btn-primary btn-block" type="submit">Вход</Button>
                    </div>
                  </form>

                </div>
              </div>
            </div>
          </div>

        </Modal.Body>

      </Modal.Dialog>
    </div>
  }
}

export default SignIn;
