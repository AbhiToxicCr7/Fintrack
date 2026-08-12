import axios from "axios";

export const FetchExpenseService = async (loggedinUserID, token, filterCategory, filterDay, filterMonth, filterYear) => {

    const base = `https://localhost:44389/api/UserExpense/GetById`;

        const urlParams = new URLSearchParams();
        urlParams.append('userId', loggedinUserID);
        if (filterCategory) urlParams.append('category', filterCategory);
        if (filterDay) urlParams.append('day', filterDay);
        if (filterMonth) urlParams.append('month', filterMonth);
        if (filterYear) urlParams.append('year', filterYear);

        const url = `${base}?${urlParams.toString()}`;

    const response = await axios.get(
        url,
        {
            headers: {
                Authorization: `Bearer ${token}`
            }
        }
    );

    return response.data;
};

export default FetchExpenseService