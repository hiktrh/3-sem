#include <iostream>
#include "FST.h"

using namespace std;

namespace FST
{
    // Конструктор для класса RELATION
    RELATION::RELATION(char c, short nn)
    {
        symbol = c;
        nnode = nn;
    }

    // Конструктор по умолчанию для класса NODE
    NODE::NODE()
    {
        n_relation = 0;
        relations = nullptr;
    }

    // Конструктор для класса NODE с параметрами
    NODE::NODE(short n, RELATION rel, ...)
    {
        n_relation = n;
        relations = new RELATION[n];
        RELATION* p = &rel;

        for (short i = 0; i < n; i++)
        {
            relations[i] = p[i];
        }
    }

    // Конструктор для класса FST
    FST::FST(const char* s, short ns, NODE n, ...)
    {
        position = -1;
        string = s;
        nstates = ns;

        nodes = new NODE[nstates];
        rstates = new short[nstates];
        memset(rstates, 0xff, sizeof(short) * nstates);
        rstates[0] = 0;

        NODE* wptr = &n;
        for (int i = 0; i < ns; i++)
        {
            nodes[i] = wptr[i];
        }
    }

    // Функция для выполнения шага в конечном автомате
    bool step(FST& fst, short*& rstates)
    {
        bool rc = false;
        swap(rstates, fst.rstates);

        for (short i = 0; i < fst.nstates; i++)
        {
            if (rstates[i] == fst.position)
            {
                for (short j = 0; j < fst.nodes[i].n_relation; j++)
                {
                    if (fst.nodes[i].relations[j].symbol == fst.string[fst.position])
                    {
                        fst.rstates[fst.nodes[i].relations[j].nnode] = fst.position + 1;
                        rc = true;
                    }
                }
            }
        }

        return rc;
    }

    // Функция для выполнения конечного автомата
    bool execute(FST& fst)
    {
        short* rstates = new short[fst.nstates];
        memset(rstates, 0xff, sizeof(short) * fst.nstates);
        short lstring = strlen(fst.string);
        bool rc = true;

        for (short i = 0; i < lstring && rc; i++)
        {
            fst.position++;
            rc = step(fst, rstates);
        }

        delete[] rstates;
        return (rc ? (fst.rstates[fst.nstates - 1] == lstring) : rc);
    }
};