import React, { useState, useRef } from 'react';
import { AiFillEyeInvisible, AiFillEye } from 'react-icons/ai';
import { Boardwise_logo_new } from '../utils/image';
import Loader from '../components/Loader';
import { accountActions } from '../store/account/accountActions';
import { useDispatch } from 'react-redux';
import Form from 'react-bootstrap/Form';
import FloatingLabel from 'react-bootstrap/FloatingLabel';
import { emailRegx, passwordRegx, nameRegx } from '../utils/validationCheck';
import { useNavigate } from 'react-router-dom';
import Amplitude from '../utils/Amplitude';
import ReCAPTCHA from "react-google-recaptcha";
import myEnv from '../utils/env.json';

const RegisterUser = () => {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const recaptcha = useRef();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [password2, setPassword2] = useState('');
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const [showPassword2, setShowPassword2] = useState(false);
  const [isError, setIsError] = useState(false);
  const [isValidEmail, setIsValidEmail] = useState(true);
  const [isValidCaptcha, setIsValidCaptcha] = useState(true);
  const [isValidPassword, setIsValidPassword] = useState(true);
  const [isValidFirstName, setIsValidFirstName] = useState(true);
  const [isValidLastName, setIsValidLastName] = useState(true);
  const [matchPasswords, setMatchPasswords] = useState(true);
  const [message, setMessage] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [isSuccess, setIsSuccess] = useState(false);

  const handleShowPassword = () => {
    setShowPassword(!showPassword);
  };
  const handleShowPassword2 = () => {
    setShowPassword2(!showPassword2);
  };

  const validateEmail = (value) => {
    const testEmail = emailRegx.exec(String(value).toLowerCase());
    if (testEmail !== null) {
      setIsValidEmail(true);
    } else {
      setIsValidEmail(false);
    }
  };

  const validatePassword = (value) => {
    const testPassword = passwordRegx.exec(value);
    if (testPassword !== null) {
      setIsValidPassword(true);
    } else {
      setIsValidPassword(false);
    }
  };

  const validateFirstName = (value) => {
    const fName = nameRegx.exec(value);
    if (fName !== null) {
      setIsValidFirstName(true);
    } else {
      setIsValidFirstName(false);
    }
  };

  const validateLastName = (value) => {
    const lName = nameRegx.exec(value);
    if (lName !== null) {
      setIsValidLastName(true);
    } else {
      setIsValidLastName(false);
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
  const validateCaptcha = () => {
   const captchaValue = recaptcha.current.getValue();
    if (!captchaValue) {
      setIsValidCaptcha(false);
    } else {
      setIsValidCaptcha(true) 
    }

  };
  
  

  const isEmpty = (value, label) => {
    if (isError && value === '') {
      return (
        <p className="errorTxt">Please enter {label}.</p>
      )
    }
  }

  const validateName = (validName, nameVal) => {
    if (!validName && nameVal !== '') {
      return <p className="errorTxt">Valid characters include A-z and ( , . ' - )</p>
    }
  };

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
    validateCaptcha();
    if (
      email !== '' && 
      password !== '' && 
      password2 !== '' && 
      firstName !== '' && 
      lastName !== '' && 
      isValidPassword &&
      isValidFirstName &&
      isValidLastName &&
      recaptcha.current.getValue()&&
      isValidEmail && matchPasswords) {
      setIsLoading(true);
      setIsError(false);
      setIsValidEmail(true);

      dispatch(accountActions.onRegisterUser(email, firstName, lastName, password,recaptcha.current.getValue())).then((res) => {
    
        if (res?.success) 
        {
          setIsSuccess(true);
          Amplitude('Registration Success', {
            email_id: `${email}`,first_name: `${firstName}`,last_name: `${lastName}`
          },email);
        }
        setIsLoading(false);
        setMessage(res.message);
      });
    } else {
      if (!isValidEmail) {
        setIsValidEmail(false);
      }
        setIsError(true);
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
            {message !== '' && <div className={`login-modal ${isSuccess ? 'message-success' : 'message-error'}`}>{message}</div>}
            
            {!isSuccess ? ( <div className="signInWrapper">
              <span className="signInTitle">Register</span>
              <span className="createAccount">or <button onClick={()=>navigate('/login')}> Sign In</button>
              </span>
              <div className="siginFormWrpper">
                <Form onSubmit={handleSubmit}>
                  <FloatingLabel label="Email address" className="mb-3">
                    <Form.Control
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
                    {isEmpty(email, "email")}
                  </FloatingLabel>

                  <br />
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
                  <br />
                  <FloatingLabel label="Confirm Password" className="mb-3 passwordInpt">
                    <Form.Control
                      type={isShowPassword(showPassword2)}
                      id="password2"
                      placeholder="Confirm Password"
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
                  <br />
                  <FloatingLabel label="First Name" className="mb-3">
                    <Form.Control
                      type="text"
                      id="firstName"
                      placeholder="First Name"
                      value={firstName}
                      onChange={(e) => {
                        setFirstName(e.target.value);
                      }}
                      maxLength={25}
                      onBlur={(e) => validateFirstName(e.target.value.trim())}
                    />
                    {validateName(isValidFirstName, firstName)}
                    {isEmpty(firstName, "first name")}
                  </FloatingLabel>
                  <br />
                  <FloatingLabel label="Last Name" className="mb-3">
                    <Form.Control
                      type="text"
                      id="lastName"
                      placeholder="Last Name"
                      value={lastName}
                      onChange={(e) => {
                        setLastName(e.target.value);
                      }}
                      maxLength={25}
                      onBlur={(e) => validateLastName(e.target.value.trim())}
                    />
                    {validateName(isValidLastName, lastName)}
                    {isEmpty(lastName, "last name")}
                  </FloatingLabel>
                  <br />
                  <ReCAPTCHA 
                    sitekey={myEnv.REACT_APP_SITE_KEY}
                    ref={recaptcha}
                    onChange={validateCaptcha} 
                    
                  />
                  {!isValidCaptcha ? (
                      <p className="errorTxt">Please fill reCAPTCHA.</p>
                    ) : null}
                  <div className="signInBtnWrapper">
                    <button className="loginBtn">
                      <span className="loginTxt">Register</span>
                    </button>
                  </div>
                  <div className="termsAndPrivacy">
                    <a href="https://www.getboardwise.com/terms-conditions" target="_blank">
                        Terms of Service
                      </a>&nbsp;and&nbsp;<a 
                      href="https://www.getboardwise.com/privacy-policy" target="_blank">
                        Privacy Policy
                      </a>
                    </div>
                </Form>
              </div>
            </div>) : (
            <div className="signInWrapper">
              <button className="backToLogin" onClick={() => navigate('/login')}>Back to sign in</button>
            </div>
            )}
            
          </div>
        </div>
      </div>
  );
};

export default RegisterUser;
