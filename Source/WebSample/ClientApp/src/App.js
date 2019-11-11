import React, { Component } from 'react';
import { Layout } from './pages/layout';
import './custom.css';
import BaseRouter from './routes';

export default class App extends Component {

    render() {
        return (
            <Layout>
                <BaseRouter />
            </Layout>
        );
    }
}
