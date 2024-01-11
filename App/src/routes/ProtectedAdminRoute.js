import { Navigate, useOutlet } from 'react-router-dom';
import { useAuth } from './useAuth';

export const ProtectedAdminRoute = () => {
  const { user } = useAuth();
  const outlet = useOutlet();
  if (user !== 'admin') {
    // user is not authenticated
    return <Navigate to="/" />;
  }
  return <div>{outlet}</div>;
};
