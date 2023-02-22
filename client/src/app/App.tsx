import * as React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Projects from "./views/Projects";
import Customers from "./views/Customers";
import Developers from "./views/Developers";
import ProjectStages from "./views/ProjectStages";
import TimeLogs from "./views/Timelogs";
import "./style.css";

export default function App() {
    return (
        <Router>
            <>
                <header className="bg-gray-900 text-white flex items-center h-16 w-full">
                    <div className="flex container mx-auto">
                        <a className="navbar-brand" href="/">
                            Timelogger
                            by
                            <svg className="b-0" id="visma_logo" data-name="visma logo" xmlns="http://www.w3.org/2000/svg" width="112.025" height="21" viewBox="0 0 112.025 21">
                            <g id="Group_1" data-name="Group 1">
                                <path id="Path_1" data-name="Path 1" d="M11.987,3.5c6.14.526,12.805,3.6,16.664,7.981,4.912,5.613,3.245,11.227-3.771,12.542S8.215,21.831,3.3,16.218C-.468,11.92-.38,7.622,3.04,5.254L22.511,19.638Z" transform="translate(-0.475 -3.5)" fill="#f01245"></path>
                            </g>
                            <g id="Group_2" data-name="Group 2" transform="translate(38.701 0.088)">
                                <path id="Path_2" data-name="Path 2" d="M121.725,24.136l-2.456-9.823a24.837,24.837,0,0,1-.7-3.508h-.088a25.686,25.686,0,0,1-.614,3.6l-2.456,9.736H111.2L116.9,3.7h3.333l5.876,20.436Z" transform="translate(-52.787 -3.612)" fill="currentColor"></path>
                                <path id="Path_3" data-name="Path 3" d="M48.9,3.6l2.456,9.823a24.836,24.836,0,0,1,.7,3.508h.088a25.686,25.686,0,0,1,.614-3.6L55.213,3.6h4.21L53.809,24.124H50.476L44.6,3.6Z" transform="translate(-44.6 -3.6)" fill="currentColor"></path>
                                <path id="Path_4" data-name="Path 4" d="M69.035,3.6V24.124H65V3.6Z" transform="translate(-47.108 -3.6)" fill="currentColor"></path>
                                <path id="Path_5" data-name="Path 5" d="M94.692,3.6l2.017,12.1h0L98.376,3.6h5.438l2.1,20.436H101.8L100.832,9.3h-.088l-.614,5.087-1.754,9.648H95.218L93.2,14.213l-.7-4.561V9.3h-.088l-.526,14.823H87.5L89.254,3.688h5.438Z" transform="translate(-49.873 -3.6)" fill="currentColor"></path>
                                <path id="Path_6" data-name="Path 6" d="M80.245,3.6a5.059,5.059,0,0,0-1.579,2.193c-1.052,2.544.088,4.473,1.052,6.227.175.263.263.526.439.789,2.193,3.771,3.07,6.49,1.4,10.437-.088.263-.351.789-.351.789H76.3a5.661,5.661,0,0,0,1.491-1.93c1.14-2.719-.175-5.175-1.491-7.543-1.491-2.894-3.157-5.789-1.4-9.911L75.421,3.6h4.824Z" transform="translate(-48.231 -3.6)" fill="currentColor"></path>
                            </g>
                            </svg>
                        </a>
                        <nav className="ml-6 space-x-6 justify-center flex w-full">
                                <a href="/time-logs" className="text-gray-300 hover:bg-gray-700 hover:text-white px-3 py-3 rounded-md text-sm font-medium">
                                Time Log
                                </a>
                                <a href="/projects" className="text-gray-300 hover:bg-gray-700 hover:text-white px-3 py-3 rounded-md text-sm font-medium">
                                Project
                                </a>
                                <a href="/customers" className="text-gray-500 hover:bg-gray-700 hover:text-white px-3 py-3 rounded-md text-sm font-medium">
                                Customer
                                </a>
                                <a href="/developers" className="text-gray-500 hover:bg-gray-700 hover:text-white px-3 py-3 rounded-md text-sm font-medium">
                                Developer
                                </a>
                                <a href="/project-stages" className="text-gray-500 hover:bg-gray-700 hover:text-white px-3 py-3 rounded-md text-sm font-medium">
                                Project Stage
                            </a>
                        </nav>
                    </div>
                </header>

                <main>
                    <div className="container mx-auto">
                        <Routes>
                            <Route path="/" element={<TimeLogs />} />
                            <Route path="/customers" element={<Customers />} />
                            <Route path="/developers" element={<Developers />} />
                            <Route path="/project-stages" element={<ProjectStages />} />
                            <Route path="/projects" element={<Projects />} />
                            <Route path="/time-logs" element={<TimeLogs />} />
                        </Routes>
                    </div>
                </main>
            </>
        </Router>
    );
}
