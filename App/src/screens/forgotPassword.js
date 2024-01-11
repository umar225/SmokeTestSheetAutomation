import React, { useState } from 'react';
import { Boardwise_logo_new } from '../utils/image';
import Loader from '../components/Loader';
import { accountActions } from '../store/account/accountActions';
import { useDispatch } from 'react-redux';
import Form from 'react-bootstrap/Form';
import FloatingLabel from 'react-bootstrap/FloatingLabel';
import { emailRegx } from '../utils/validationCheck';
import { useNavigate } from 'react-router-dom';
import Amplitude from '../utils/Amplitude';

const ForgotPassword = () => {
  const navigate = useNavigate();
  const dispatch = useDispatch();

  const [email, setEmail] = useState('');
  const [isError, setIsError] = useState(false);
  const [isValidEmail, setIsValidEmail] = useState(true);
  const [message, setMessage] = useState('');
  const [isSuccess, setIsSuccess] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  const validateEmail = (value) => {
    const testEmail = emailRegx.exec(String(value).toLowerCase());
    if (testEmail !== null) {
      setIsValidEmail(true);
    } else {
      setIsValidEmail(false);
    }
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (email !== '' && isValidEmail) {
      setIsLoading(true);
      setIsError(false);
      setIsValidEmail(true);
      dispatch(accountActions.onForgotPassword(email)).then((res) => {
        if (res?.success) 
        {
          setIsSuccess(true);
          Amplitude(
            'Forgot Password Reset Success',
            {
              Email: email,
             
            },
            email
          );
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
            <div className="signInWrapper">.
            {!isSuccess ? (
            <div>
              <span className="signInTitle">Forgot your password?</span>
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

                    {isError && email === '' ? (
                      <p className="errorTxt">Please enter email.</p>
                    ) : null}
                  </FloatingLabel>

                  <br />

                  <div className="signInBtnWrapper">
                    <button className="loginBtn">
                      <span className="loginTxt">Request reset link</span>
                    </button>
                  </div>
                  <button className="backToLogin" onClick={() => navigate('/login')}>Back to sign in</button>
                  <div className="termsAndPrivacy">
                    <a 
                    href="https://www.getboardwise.com/terms-conditions" target="_blank"
                    >Terms of Service</a>&nbsp;and&nbsp;<a 
                    href="https://www.getboardwise.com/privacy-policy" target="_blank"
                    >Privacy Policy</a></div>
                </Form>
              </div>
            </div>
            ) : (
              <button className="backToLogin" onClick={() => navigate('/login')}>Back to sign in</button>
            )}
            </div>
          </div>
        </div>
      </div>
  );
};

export default ForgotPassword;