import React from 'react';
import Container from "react-bootstrap/Container";

const FormCompleted = () => {
    React.useEffect(() => {
        window.scrollTo(0, 0)
      }, [])
 return (
    
    <div className="whiteBackground" >
    <Container className='formcomplete'>
        <p>Thank you for completing the form!</p>
        <a href="https://getcoursewise.com">
        <button>Back to our Home Page</button>
        </a>
    </Container>
    
  </div>
 )
}

export default FormCompleted;