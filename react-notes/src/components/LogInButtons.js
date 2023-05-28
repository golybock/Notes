import { useContext } from "react";
import Button from 'react-bootstrap/Button';
import { useLocation, useNavigate } from "react-router";
import { UserContext } from "../App";

const LogInButtons = () => {
  const { user, setUser } = useContext(UserContext);
  const navigate = useNavigate();
  const location = useLocation();
  return (
    <div>
      <label>{`Logged In: ${user.loggedIn}`}</label>
        <Button
            onClick={() => {
                if (user.loggedIn) return;
                setUser({ loggedIn: true });

                if (location.state?.from) {
                    navigate(location.state.from);
                }
            }}
        >
            Log In
        </Button>
        <Button
            onClick={() => {
                if (!user.loggedIn) return;
                setUser({ loggedIn: false });
            }}
        >
            Log Out
        </Button>
    </div>
  );
};

export default LogInButtons;
