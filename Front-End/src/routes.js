import React from 'react';
import {BrowserRouter, Route, Switch} from 'react-router-dom';

import Ninjas from './pages/Ninjas'
import NewNinja from './pages/NewNinja';

export default function Routes() {
    return (
        <BrowserRouter>
            <Switch>
                <Route path="/" exact component ={Ninjas}/>
                <Route path="/ninja/new/:ninjaId" component ={NewNinja}/>
            </Switch>
        </BrowserRouter>
    )
}