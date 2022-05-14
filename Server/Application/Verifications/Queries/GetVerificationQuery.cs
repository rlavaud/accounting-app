﻿using System;

using Accounting.Application.Accounts;
using Accounting.Application.Common.Interfaces;
using Accounting.Application.Verifications;

using MediatR;

using Microsoft.EntityFrameworkCore;

using static Accounting.Application.Shared;

namespace Accounting.Application.Verifications.Queries
{
    public class GetVerificationQuery : IRequest<VerificationDto>
    {
        public GetVerificationQuery(int verificationId)
        {
            VerificationId = verificationId;
        }

        public int VerificationId { get; }

        public class GetVerificationQueryHandler : IRequestHandler<GetVerificationQuery, VerificationDto>
        {
            private readonly IAccountingContext context;

            public GetVerificationQueryHandler(IAccountingContext context)
            {
                this.context = context;
            }

            public async Task<VerificationDto> Handle(GetVerificationQuery request, CancellationToken cancellationToken)
            {
                var v = await context.Verifications
                    .Include(x => x.Entries)
                    .Include(x => x.Attachments)
                    .OrderBy(x => x.Date)
                    .AsNoTracking()
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(x => x.Id == request.VerificationId, cancellationToken);

                if (v is null) throw new Exception();

                return v.ToDto();
            }
        }
    }
}