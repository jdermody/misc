import React from 'react';
import { Person } from './data/Models';
import { CurrentMovieList } from './CurrentMovieList';
import { connect } from 'react-redux';
import { setMovies } from './data/Store.';
import { MovieSearch } from './MovieSearch';
import GraphClient from './data/GraphClient';

export interface MoviesContainerProps {
    star: string;
    client: GraphClient;
}

interface _MoviesContainerProps extends MoviesContainerProps {
    setMovies: typeof setMovies;
}

interface MoviesContainerState {
    person: Person|null;
}

class _MoviesContainer extends React.Component<_MoviesContainerProps, MoviesContainerState> {
    constructor(props:_MoviesContainerProps, state: MoviesContainerState) {
        super(props);
        this.state = {
            person: null
        };
    }

    async componentDidMount() {
        const {star, setMovies, client} = this.props;
        const id = await client.getPersonId(star);
        console.log(`${star} has id: ${id}`);
        
        const person = await client.getPersonWithMovies(id);
        this.setState({
            person
        });
        setMovies(person.appearsIn);
    }

    renderPerson(person: Person) {
        return [
            <header key="header">
                <MovieSearch name={person.name}/>
            </header>,
            <main key="main">
                <CurrentMovieList/>
            </main>
        ];
    }

    render() {
        const {person} = this.state;
        return person
            ? this.renderPerson(person)
            : <div className="loader"></div>;
    }
}

export const MoviesContainer = connect(
    undefined,
    {setMovies}
)(_MoviesContainer);