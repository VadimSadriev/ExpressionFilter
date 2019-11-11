import React from 'react';
import { Route, Switch } from 'react-router-dom';
import Home from './pages/home';

class Routers extends React.Component {

    render() {
        return (
            <Switch>
                <Route exact path="/" component={Home}></Route>
            </Switch>
        );
    }
}

export default Routers;