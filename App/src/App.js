import React from 'react';
import './App.css';
import { CusRoutes } from './routes';
import Container from 'react-bootstrap/Container';

function App() {
  return (
    <Container fluid className="p-3 my-container">
      <Container fluid className="p-5 mb-4 bg-light rounded-3 my-container">
        <div className="ft-relativ">
          <CusRoutes />
        </div>
      </Container>
    </Container>
  );
}

export default App;
