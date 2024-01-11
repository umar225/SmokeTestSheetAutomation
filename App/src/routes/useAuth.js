import { createContext, useContext, useMemo } from 'react';
import { useNavigate } from 'react-router-dom';
import { useLocalStorage } from './useLocalStorage';
import PropTypes from 'prop-types';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useLocalStorage('user', null);
  const [destinationPath, setDestinationPath] = useLocalStorage(
    'destinationPath',
    ''
  );
  const navigate = useNavigate();

  const login = async (data) => {
    setUser(data);
    if (data === 'admin') {
      navigate('/admindashboard', { replace: true });
    } 
    else if (destinationPath) {
      navigate(destinationPath, { replace: true });
    } else {
      navigate('/userdashboard', { replace: true });
    }
  };

  const logout = () => {
    setUser(null);
    setDestinationPath('');
  };

  const onDestinationPath = (value) => {
    setDestinationPath(value);
  };

  const value = useMemo(
    () => ({
      user,
      login,
      logout,
      onDestinationPath,
    }),
    [user, destinationPath]
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export const useAuth = () => {
  return useContext(AuthContext);
};
AuthProvider.propTypes = {
  children: PropTypes.node,
};