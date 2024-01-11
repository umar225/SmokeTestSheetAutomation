import React, { useState } from 'react';
import { AiFillEyeInvisible, AiFillEye } from 'react-icons/ai';
import { Boardwise_logo_new } from '../utils/image';
import Loader from '../components/Loader';
import { accountActions } from '../store/account/accountActions';
import { useDispatch } from 'react-redux';
import Form from 'react-bootstrap/Form';
import { passwordRegx } from '../utils/validationCheck';
import FloatingLabel from 'react-bootstrap/FloatingLabel';
import { useNavigate } from 'react-router-dom';

const ResetPassword = () => {
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const [password, setPassword] = useState('');
  const [password2, setPassword2] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const [showPassword2, setShowPassword2] = useState(false);
  const [isError, setIsError] = useState(false);
  const [message, setMessage] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [isValidPassword, setIsValidPassword] = useState(true);
  const [matchPasswords, setMatchPasswords] = useState(true);
  const [isSuccess, setIsSuccess] = useState(false);

  const urlString = window.location.search;
  const urlParams = new URLSearchParams(urlString);
  const userId = urlParams.get('userId');
  let token = urlParams.get('token');
  token = token.replace(/ /g, '+');

  const handleShowPassword = () => {
    setShowPassword(!showPassword);
  };
  const handleShowPassword2 = () => {
    setShowPassword2(!showPassword2);
  };

  const validatePassword = (value) => {
    const testPassword = value.match(passwordRegx);
    if (testPassword !== null) {
      setIsValidPassword(true);
    } else {
      setIsValidPassword(false);
    }
  };

  const confirmPasswords = () => {
    if (password !== password2) {
      setMatchPasswords(false);
    }
    else {
      setMatchPasswords(true);
    }
  }

  const isEmpty = (value, label) => {
    if (isError && value === '') {
      return (
        <p className="errorTxt">Please enter {label}.</p>
      )
    }
  }

  const isShowPassword = (val) => {
    if (val) {
      return ('text');
    }
    else {
      return ('password');
    }
  }

  const handleSubmit = (e) => {
    e.preventDefault();
    confirmPasswords();
    if (password && password2 && isValidPassword) {
      if (password !== password2) {
        setMessage("Passwords don't match")
      }
      else {
        setIsLoading(true);
        setIsError(false);
      dispatch(accountActions.onResetPassword(userId, token, password)).then((res) => {
        if (res?.success) setIsSuccess(true);
        setIsLoading(false);
        setMessage(res.message);
      });
      }
    } else {
      setIsError(true);
    }
  };
  return (
    <div className="mainWrapper">
      {isLoading && <Loader />}
        <div className="grayBackground">
          <div className="loginWrapper">
            <div className="loginHeader">
              <img className="bLogo" src={Boardwise_logo_new} alt="" />
            </div>
            {message !== '' && <div className={`login-modal ${isSuccess ? 'message-success' : 'message-error'}`}>{message}</div>}
            <div className="signInWrapper">
              {!isSuccess ? ( <div>
                <span className="signInTitle">Reset Password</span>
                <div className="siginFormWrpper">
                  <Form onSubmit={handleSubmit}>
                    <FloatingLabel label="Password" className="mb-3 passwordInpt">
                      <Form.Control
                        type={isShowPassword(showPassword)}
                        id="password"
                        placeholder="Password"
                        value={password}
                        onChange={(e) => {
                          setPassword(e.target.value);
                        }}
                        onBlur={(e) => validatePassword(e.target.value)}
                      />
                        <div className="pwdRules">Your password needs to:
                          <ul>
                            <li>include both upper and lower case letters</li>
                            <li>include at least one number</li>
                            <li>include at least one symbol</li>
                            <li>be minimum 8 and maximum 16 characters long</li>
                          </ul>
                        </div>
                      {isEmpty(password, "password")}
                      {showPassword ? (
                        <AiFillEyeInvisible onClick={handleShowPassword} />
                      ) : (
                        <AiFillEye onClick={handleShowPassword} />
                      )}
                    </FloatingLabel>
                    <FloatingLabel label="Re-enter Password" className="mb-3 passwordInpt">
                      <Form.Control
                        type={isShowPassword(showPassword2)}
                        id="password2"
                        placeholder="Re-enter Password"
                        value={password2}
                        onChange={(e) => {
                          setPassword2(e.target.value);
                        }}
                        onBlur={() => confirmPasswords()}
                      />
                      {isEmpty(password2, "password")}
                      {!matchPasswords && password2 ? (<p className="errorTxt">Passwords don't match</p>) : null}
                      {showPassword2 ? (
                        <AiFillEyeInvisible onClick={handleShowPassword2} />
                      ) : (
                        <AiFillEye onClick={handleShowPassword2} />
                      )}
                    </FloatingLabel>
                    <div className="signInBtnWrapper">
                      <button className="loginBtn">
                        <span className="loginTxt">Submit</span>
                      </button>
                    </div>
                    <button className="backToLogin" onClick={() => navigate('/login')}>Back to sign in</button>
                    <div className="termsAndPrivacy">
                      <a href="https://www.getboardwise.com/terms-conditions" target="_blank">
                          Terms of Service
                        </a>&nbsp;and&nbsp;<a 
                        href="https://www.getboardwise.com/privacy-policy" target="_blank">
                          Privacy Policy
                        </a></div>
                  </Form>
                </div>
              </div>
              ) : (
              <div>
                <button className="loginBtn"  onClick={() => navigate('/login')}>
                  <span className="loginTxt">Sign In</span>
                </button>
            </div>)}
            </div>
          </div>
        </div>
      </div>
  );
};

export default ResetPassword;
