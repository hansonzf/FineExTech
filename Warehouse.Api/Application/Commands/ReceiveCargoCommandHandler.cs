using MediatR;
using Warehouse.Domain.AggregatesModel.StorehouseAggregate;

namespace Warehouse.Api.Application.Commands
{
    public class ReceiveCargoCommandHandler
        : IRequestHandler<ReceiveCargoCommand, bool>
    {
        private readonly IStorehouseRepository _repository;
        private readonly IMediator _mediator;
        private readonly ILogger<ReceiveCargoCommandHandler> _logger;

        public ReceiveCargoCommandHandler(
            IStorehouseRepository repository, 
            IMediator mediator, 
            ILogger<ReceiveCargoCommandHandler> logger)
        {
            _repository = repository;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<bool> Handle(ReceiveCargoCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
