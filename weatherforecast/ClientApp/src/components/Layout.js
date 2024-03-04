import React, { Component } from 'react';
import { Container } from 'reactstrap';
import NavMenu from './NavMenu';
import LocationSearch from './LocationSearch';

function Layout(props){
    //const [cookies, setCookie] = useCookies(["user"]);
    
    return (
      <div style={{ "background-color": "#f5f5f2","width":"1500px", "margin":"0 auto"}}>        
        <LocationSearch />
        <NavMenu />
        <Container fluid>
          { props.children}
        </Container>
      </div>
    );
  
}


export default Layout;
