import React, { useState, useEffect } from 'react';
import { AiFillEyeInvisible, AiFillEye } from 'react-icons/ai';
import { Boardwise_logo_new } from '../utils/image';
import Loader from '../components/Loader';
import { accountActions } from '../store/account/accountActions';
import { useDispatch } from 'react-redux';
import Form from 'react-bootstrap/Form';
import FloatingLabel from 'react-bootstrap/FloatingLabel';
import Amplitude from '../utils/Amplitude';
import { emailRegx } from '../utils/validationCheck';
import { useAuth } from '../routes/useAuth';
import { useNavigate, useLocation } from 'react-router-dom';
import { toast } from 'react-toastify';
import LinkedInIcon from '@mui/icons-material/LinkedIn';
import GoogleIcon from '@mui/icons-material/Google';

import ErrorOutlineIcon from '@mui/icons-material/ErrorOutline';

import { useGoogleLogin } from '@react-oauth/google';

const Login = () => {
  const { login } = useAuth();
  const dispatch = useDispatch();
  const location = useLocation();
  const navigate = useNavigate();

  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const [isError, setIsError] = useState(false);
  const [isValidEmail, setIsValidEmail] = useState(true);
  const [message, setMessage] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [isSuccess, setIsSuccess] = useState(false);
  useEffect(() => {
    if (location && location.search === '?isError') {
      toast.error('Your session has expired');
    }
    let isLoggedIn = localStorage.getItem('access_token');
    let userRole = localStorage.getItem('user');
    if (isLoggedIn && isLoggedIn !== null && userRole == "customer") {
      navigate("/userdashboard")
    }
  }, []);

  const handleShowPassword = () => {
    setShowPassword(!showPassword);
  };

  const validateEmail = (value) => {
    const testEmail = emailRegx.exec(String(value).toLowerCase());
    if (testEmail !== null) {
      setIsValidEmail(true);
    } else {
      setIsValidEmail(false);
    }
  };
  const googleLogin = useGoogleLogin({
    onSuccess: (codeResponse) => googleAuth(codeResponse),
    onError: (error) => console.log('Login Failed:', error)
  });
  const googleAuth = (codeResponse) => {
    setIsLoading(true);
    dispatch(accountActions.onExternalLogin('google', codeResponse.access_token)).then((res) => {
      setMessage(res.message);
      setIsLoading(false);
      if (res?.success) {
        if (res.data?.role === 'customer') {
          login('customer');
        }
      }
    })
  }
  const handleSubmit = (e) => {
    e.preventDefault();
    if (email !== '' && password !== '' && isValidEmail) {
      setIsLoading(true);
      setIsError(false);
      setIsValidEmail(true);
      dispatch(accountActions.onUserLogin(email, password)).then((res) => {
        setMessage(res.message);
        setIsLoading(false);
        if (res?.success) {
          setIsSuccess(true);
          if (res.data?.role === 'customer') {
            Amplitude(
              'Login Success',
              {
                Email: email,
                userId: res.data?.userId,
              },
              email
            );
            login('customer');
          } else {
            login('admin');
          }
        } else {
          Amplitude('Login unsuccessfully');
        }
      });
    } else {
      if (!isValidEmail) {
        setIsValidEmail(false);
      }
      setIsError(true);
    }
  };

  const styles = {
    formControl: {
      outline: 'none', 
      boxShadow: 'none', 
      borderColor: 'transparent', 
      ':focus': {
        borderColor: 'transparent'
      }
    }
  }

  return (
    <div className="mainWrapper">
      {isLoading && <Loader />}
      <div className="grayBackground">
        <div className="loginWrapper">
          <div className="loginHeader">
            <img className="bLogo" src={Boardwise_logo_new} alt="" />
          </div>
          <div className="signInWrapper">
            <span className="signInTitle">Sign in</span>
            <span className="createAccount">
              Don't have an account? <button onClick={() => navigate('/registeruser')}>Register</button></span>
              {message !== '' && <div className={`login-modal loginMessage ${isSuccess ? 'message-success' : 'message-error'}`}>
            <ErrorOutlineIcon style={{ marginRight: 12, }} />
            <div>
              {message}
            </div>
          </div>}
            <div className="siginFormWrpper">
              <Form onSubmit={handleSubmit}>
                <FloatingLabel label="Email address" className="mb-3">
                  <Form.Control
                    style={styles.formControl}
                    type="email"
                    id="email"
                    placeholder="Email Address"
                    value={email}
                    onChange={(e) => {
                      setEmail(e.target.value);
                    }}
                    onBlur={(e) => validateEmail(e.target.value)}
                  />
                  {!isValidEmail && email !== '' ? (
                    <p className="errorTxt">Please enter valid email.</p>
                  ) : null}

                  {isError && email === '' ? (
                    <p className="errorTxt">Please enter email.</p>
                  ) : null}
                </FloatingLabel>

                <FloatingLabel label="Password" className="mb-3 passwordInpt">
                  <Form.Control
                    style={styles.formControl}
                    type={showPassword ? 'text' : 'password'}
                    id="password"
                    placeholder="My Password"
                    value={password}
                    onChange={(e) => {
                      setPassword(e.target.value);
                    }}
                  />
                  {isError && password === '' ? (
                    <p className="errorTxt">Please enter password.</p>
                  ) : null}
                  {showPassword ? (
                    <AiFillEyeInvisible onClick={handleShowPassword} />
                  ) : (
                    <AiFillEye onClick={handleShowPassword} />
                  )}
                </FloatingLabel>
                <div className="rememberWrapper">
                  {/* <span>
                      <Form.Group>
                        <div className="crsLevelWrapper">
                          <Form.Check
                            className="rememberChkBox"
                            type={'checkbox'}
                            label={'Remember me'}
                            // checked={checkCrsLevel(lvl.id)}
                            // onChange={() => selectCrsLevel(lvl.id)}
                          />
                        </div>
                      </Form.Group>
                    </span> */}
                  <button className="forgotTxt" onClick={() => navigate('/forgotpassword')} type="button">
                    Forgot Password
                  </button>
                  <button className="forgotTxt" onClick={() => navigate('/resendConfirmation')} type="button">
                    Resend verification email
                  </button>
                </div>
                <div className="signInBtnWrapper">
                  <button
                    type="submit"
                    className="loginBtn"
                    id="loginBtn"
                  // onClick={(e) => handleSubmit(e)}
                  >
                    <span className="loginTxt">Log in</span>
                  </button>
                </div>

              </Form>
              <div className="separator"><p>OR</p></div>
              <div className="signInBtnWrapper signInBtnWrapperGoogle">
                <button className="loginBtn SSOLogin" onClick={() => googleLogin()}>
                  <span className="loginTxt">
                    <GoogleIcon sx={{ fontSize: "24px", margin: "0 7px 2px 0" }} />
                    Continue with Google
                  </span>
                </button>
              </div>
              <div className="signInBtnWrapper signInBtnWrapperLinkedin">
                <button className="loginBtn SSOLogin">
                  <a
                    href={`https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id=78h8sqdxng72o6&redirect_uri=${window.location.origin + "/linkedin-redirect"}&scope=profile%20email%20openid`}
                  >
                    <span className="loginTxt">
                      <LinkedInIcon sx={{ fontSize: "24px", margin: "0 7px 2px 0" }} />
                      Continue with LinkedIn
                    </span>
                  </a>
                </button>
              </div>
              <div className="termsAndPrivacy">
                <a 
                href="https://www.getboardwise.com/terms-conditions" target="_blank">
                  Terms of Service</a>&nbsp;and&nbsp;<a 
                href="https://www.getboardwise.com/privacy-policy" target="_blank">
                  Privacy Policy
                </a>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Login;
