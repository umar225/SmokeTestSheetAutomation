import { Navigate, useOutlet } from 'react-router-dom';
import { useAuth } from './useAuth';

export const ProtectedPublicRoute = () => {
  const { user, onDestinationPath } = useAuth();
  const outlet = useOutlet();

  if (user !== 'customer') {
    const destinationPath = window.location.pathname;
    onDestinationPath(destinationPath);
    return <Navigate to="/login" replace />;
  }

  return <div>{outlet}</div>;
};
