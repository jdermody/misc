import React from 'react';
import ReactDOM from 'react-dom';
import { MoviesContainer } from './MoviesContainer';
import { Provider } from 'react-redux';
import { createStore } from "redux";
import { moviesReducer } from './data/Store.';
import GraphClient from './data/GraphClient';

const store = createStore(
    moviesReducer,
    (window as any).__REDUX_DEVTOOLS_EXTENSION__ && (window as any).__REDUX_DEVTOOLS_EXTENSION__()
);

const client = new GraphClient('https://112qaej5y9.execute-api.ap-southeast-2.amazonaws.com/dev/graphql');

const container = document.getElementById("main");
ReactDOM.render(
    <Provider store={store}>
        <MoviesContainer 
            star="tom cruise"
            client={client} 
        />
    </Provider>    
, container);