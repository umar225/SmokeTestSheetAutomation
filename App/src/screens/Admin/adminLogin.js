import React, { useState, useEffect } from 'react';
import { AiFillEyeInvisible, AiFillEye } from 'react-icons/ai';
import { Boardwise_logo_new } from '../../utils/image';
import Loader from '../../components/Loader';
import { accountActions } from '../../store/account/accountActions';
import { useDispatch } from 'react-redux';
import Form from 'react-bootstrap/Form';
import FloatingLabel from 'react-bootstrap/FloatingLabel';
import Amplitude from '../../utils/Amplitude';
import { emailRegx } from '../../utils/validationCheck';
import { useAuth } from '../../routes/useAuth';
import { useLocation } from 'react-router-dom';
import { toast } from 'react-toastify';

const AdminLogin = () => {
  const { login } = useAuth();
  const dispatch = useDispatch();
  const location = useLocation();

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
    Amplitude('Webapp login ');
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

  const handleSubmit = (e) => {
    e.preventDefault();
    if (email !== '' && password !== '' && isValidEmail) {
      setIsLoading(true);
      setIsError(false);
      setIsValidEmail(true);
      dispatch(accountActions.onAdminLogin(email, password)).then((res) => {
        setMessage(res.message);
        setIsLoading(false);
        if (res?.success) {
          setIsSuccess(true);
          if (res.data?.role === 'admin') {
            Amplitude(
              'Admin Login successfully',
              {
                Email: email,
                userId: res.data?.userId,
              },
              email
            );
            login('admin');
          } else {
            login('customer');
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
              <span className="signInTitle">Admin Sign in</span>
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

                  <FloatingLabel label="Password" className="mb-3 passwordInpt">
                    <Form.Control
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
                  <div className="signInBtnWrapper">
                    <button
                      className="loginBtn"
                    >
                      <span className="loginTxt">Log in</span>
                    </button>
                  </div>
                </Form>
              </div>
            </div>
          </div>
        </div>
      </div>
  );
};

export default AdminLogin;
