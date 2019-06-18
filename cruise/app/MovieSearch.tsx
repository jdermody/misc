import React from 'react';
import { connect } from 'react-redux';
import { setQuery, StoreState } from './data/Store.';

export interface MovieSearchProps {
    name: string;
    query: string;
}

interface _MovieSearchProps extends MovieSearchProps {
    setQuery: typeof setQuery;
}

interface MovieSearchState {
    searchText: string;
}

class _MovieSearch extends React.Component<_MovieSearchProps, MovieSearchState> {
    waitId = 0;
    searchInput: HTMLInputElement|null = null;

    constructor(props: _MovieSearchProps) {
        super(props);
        this.state = {
            searchText: ""
        };
    }

    searchForMovies() {
        const {searchText} = this.state;
        console.log(`searching for ${searchText}`);
        this.props.setQuery(searchText);
    }

    render() {
        const {name, query} = this.props;
        const {searchText} = this.state;
        const suffix = (query && query.length) ? ` (movies that match "${query}")` : '';
        return <section>
            <h1>{name}{suffix}</h1>
            <div className="searchbox">
                <img 
                    src="data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiA/PjwhRE9DVFlQRSBzdmcgIFBVQkxJQyAnLS8vVzNDLy9EVEQgU1ZHIDEuMS8vRU4nICAnaHR0cDovL3d3dy53My5vcmcvR3JhcGhpY3MvU1ZHLzEuMS9EVEQvc3ZnMTEuZHRkJz48c3ZnIGVuYWJsZS1iYWNrZ3JvdW5kPSJuZXcgMCAwIDUwIDUwIiBoZWlnaHQ9IjUwcHgiIGlkPSJMYXllcl8xIiB2ZXJzaW9uPSIxLjEiIHZpZXdCb3g9IjAgMCA1MCA1MCIgd2lkdGg9IjUwcHgiIHhtbDpzcGFjZT0icHJlc2VydmUiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgeG1sbnM6eGxpbms9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkveGxpbmsiPjxyZWN0IGZpbGw9Im5vbmUiIGhlaWdodD0iNTAiIHdpZHRoPSI1MCIvPjxjaXJjbGUgY3g9IjIxIiBjeT0iMjAiIGZpbGw9Im5vbmUiIHI9IjE2IiBzdHJva2U9IiMwMDAwMDAiIHN0cm9rZS1saW5lY2FwPSJyb3VuZCIgc3Ryb2tlLW1pdGVybGltaXQ9IjEwIiBzdHJva2Utd2lkdGg9IjIiLz48bGluZSBmaWxsPSJub25lIiBzdHJva2U9IiMwMDAwMDAiIHN0cm9rZS1taXRlcmxpbWl0PSIxMCIgc3Ryb2tlLXdpZHRoPSI0IiB4MT0iMzIuMjI5IiB4Mj0iNDUuNSIgeTE9IjMyLjIyOSIgeTI9IjQ1LjUiLz48L3N2Zz4=" 
                    alt="Search" 
                    onClick={e => this.searchInput && this.searchInput.focus()}
                />
                <input 
                    type="search" 
                    ref={el => this.searchInput = el}
                    value={searchText} 
                    autoFocus={true}
                    onChange={e => {
                        this.setState({searchText: e.currentTarget.value});
                        if(this.waitId)
                            clearTimeout(this.waitId);
                        this.waitId = setTimeout(() => this.searchForMovies(), 500);
                    }}
                />
            </div>
        </section>;
    }
}

export const MovieSearch = connect((state: StoreState) => ({query: state.query}), {setQuery})(_MovieSearch);